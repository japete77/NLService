Imports System.ServiceModel.Channels
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher

Namespace NuevaLuz.AudioBooks
    Public Class MessageInspector
        Implements IDispatchMessageInspector
        Private _serviceEndpoint As ServiceEndpoint

        Public Sub New(serviceEndpoint As ServiceEndpoint)
            _serviceEndpoint = serviceEndpoint
        End Sub

        ''' <summary>
        ''' Called when an inbound message been received
        ''' </summary>
        ''' <param name="request">The request message.</param>
        ''' <param name="channel">The incoming channel.</param>
        ''' <param name="instanceContext">The current service instance.</param>
        ''' <returns>
        ''' The object used to correlate stateMsg. 
        ''' This object is passed back in the method.
        ''' </returns>
        Private Function IDispatchMessageInspector_AfterReceiveRequest(ByRef request As Message, channel As IClientChannel, instanceContext As InstanceContext) As Object Implements IDispatchMessageInspector.AfterReceiveRequest
            Dim stateMsg As StateMessage = Nothing
            Dim requestProperty As HttpRequestMessageProperty = Nothing
            If request.Properties.ContainsKey(HttpRequestMessageProperty.Name) Then
                requestProperty = TryCast(request.Properties(HttpRequestMessageProperty.Name), HttpRequestMessageProperty)
            End If

            If requestProperty IsNot Nothing Then
                Dim origin = requestProperty.Headers("Origin")
                If Not String.IsNullOrEmpty(origin) Then
                    stateMsg = New StateMessage()
                    ' if a cors options request (preflight) is detected, 
                    ' we create our own reply message and don't invoke any 
                    ' operation at all.
                    If requestProperty.Method = "OPTIONS" Then
                        stateMsg.Message = Message.CreateMessage(request.Version, Nothing)
                    End If
                    request.Properties.Add("CrossOriginResourceSharingState", stateMsg)
                End If
            End If

            Return stateMsg
        End Function

        ''' <summary>
        ''' Called after the operation has returned but before the reply message
        ''' is sent.
        ''' </summary>
        ''' <param name="reply">The reply message. This value is null if the 
        ''' operation is one way.</param>
        ''' <param name="correlationState">The correlation object returned from
        '''  the method.</param>
        Private Sub IDispatchMessageInspector_BeforeSendReply(ByRef reply As Message, correlationState As Object) Implements IDispatchMessageInspector.BeforeSendReply
            Dim stateMsg = TryCast(correlationState, StateMessage)

            If stateMsg IsNot Nothing Then
                If stateMsg.Message IsNot Nothing Then
                    reply = stateMsg.Message
                End If

                Dim responseProperty As HttpResponseMessageProperty = Nothing

                If reply.Properties.ContainsKey(HttpResponseMessageProperty.Name) Then
                    responseProperty = TryCast(reply.Properties(HttpResponseMessageProperty.Name), HttpResponseMessageProperty)
                End If

                If responseProperty Is Nothing Then
                    responseProperty = New HttpResponseMessageProperty()
                    reply.Properties.Add(HttpResponseMessageProperty.Name, responseProperty)
                End If

                ' Access-Control-Allow-Origin should be added for all cors responses
                responseProperty.Headers.[Set]("Access-Control-Allow-Origin", "*")

                If stateMsg.Message IsNot Nothing Then
                    ' the following headers should only be added for OPTIONS requests
                    responseProperty.Headers.[Set]("Access-Control-Allow-Methods", "POST, OPTIONS, GET")
                    responseProperty.Headers.[Set]("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, x-requested-with")
                End If
            End If
        End Sub
    End Class

    Class StateMessage
        Public Message As Message
    End Class

    Public Class BehaviorAttribute
        Inherits Attribute
        Implements IEndpointBehavior
        Implements IOperationBehavior

        Private Sub IEndpointBehavior_Validate(endpoint As ServiceEndpoint) Implements IEndpointBehavior.Validate
        End Sub

        Private Sub IEndpointBehavior_AddBindingParameters(endpoint As ServiceEndpoint, bindingParameters As BindingParameterCollection) Implements IEndpointBehavior.AddBindingParameters
        End Sub

        ''' <summary>
        ''' This service modify or extend the service across an endpoint.
        ''' </summary>
        ''' <param name="endpoint">The endpoint that exposes the contract.</param>
        ''' <param name="endpointDispatcher">The endpoint dispatcher to be
        ''' modified or extended.</param>
        Private Sub IEndpointBehavior_ApplyDispatchBehavior(endpoint As ServiceEndpoint, endpointDispatcher As EndpointDispatcher) Implements IEndpointBehavior.ApplyDispatchBehavior
            ' add inspector which detects cross origin requests
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(New MessageInspector(endpoint))
        End Sub

        Private Sub IEndpointBehavior_ApplyClientBehavior(endpoint As ServiceEndpoint, clientRuntime As ClientRuntime) Implements IEndpointBehavior.ApplyClientBehavior
        End Sub

        Private Sub IOperationBehavior_Validate(operationDescription As OperationDescription) Implements IOperationBehavior.Validate
        End Sub

        Private Sub IOperationBehavior_ApplyDispatchBehavior(operationDescription As OperationDescription, dispatchOperation As DispatchOperation) Implements IOperationBehavior.ApplyDispatchBehavior
        End Sub

        Private Sub IOperationBehavior_ApplyClientBehavior(operationDescription As OperationDescription, clientOperation As ClientOperation) Implements IOperationBehavior.ApplyClientBehavior
        End Sub

        Private Sub IOperationBehavior_AddBindingParameters(operationDescription As OperationDescription, bindingParameters As BindingParameterCollection) Implements IOperationBehavior.AddBindingParameters
        End Sub
    End Class
End Namespace