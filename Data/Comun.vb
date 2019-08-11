Imports System.Configuration
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web

Public Module Comun

    Public Function GetConnectionString(ByVal connName As String) As String
        Dim _conn As String = ConfigurationManager.ConnectionStrings(connName).ConnectionString.ToString()
        If String.IsNullOrEmpty(_conn) Then
            Throw New ApplicationException("No existe conexion a la base de datos definida.")
        End If
        Return _conn
    End Function

    Public Function GetConnString() As String
        Dim _conn As String = ConfigurationManager.ConnectionStrings("SenalesCaribeConnection").ConnectionString.ToString()
        Return _conn
    End Function

    Public Function SendMail(ByVal emailTo As String, ByVal messageContent As String, ByVal messageSubject As String) As String
        Return SendMail(emailTo, messageContent, messageSubject, "info@SenalesCaribeConnection.com.do")
    End Function

    Public Function SendMail(ByVal emailTo As String, ByVal messageContent As String, ByVal messageSubject As String, ByVal emailFrom As String) As String
        Dim _result As String = String.Empty
        Try
            Dim smtp_mensaje As New SmtpClient()
            Using mensaje As New MailMessage
                With mensaje
                    .From = New MailAddress(emailFrom, "Senales del Caribe")
                    .To.Add(emailTo)
                    .Subject = messageSubject
                    .IsBodyHtml = True
                    .Body = messageContent
                End With
                smtp_mensaje.Send(mensaje)
            End Using

            _result = String.Format("Se ha enviado el correo a ({0}) satisfactoriamente.", emailTo)
        Catch ex As Exception
            _result = "Ha ocurrido un error al enviar el correo, vuelva a intentarlo mas tarde:" + ex.Message.ToString
        End Try
        Return _result
    End Function

    Public Function GetParametroCustomString(ByVal _nombre As String) As String
        Dim _result As String = String.Empty

        Try
            Dim db As New SecurityDataContext(GetConnString)

            Dim _param = db.Parametros.FirstOrDefault(Function(d) d.Descripcion = _nombre)

            If Not IsNothing(_param) Then
                _result = _param.CustomString
            Else
                _result = String.Empty
            End If
        Catch ex As Exception
            'to do
        End Try

        Return _result
    End Function

    Public Function GetParametroInt1(ByVal _nombre As String) As Integer
        Dim _result As Integer = -1

        Try
            Dim db As New SecurityDataContext(GetConnString)

            Dim _param = db.Parametros.FirstOrDefault(Function(d) d.Descripcion = _nombre)

            If Not IsNothing(_param) Then
                _result = _param.CustomInt1
            Else
                _result = String.Empty
            End If
        Catch ex As Exception
            'to do
        End Try

        Return _result
    End Function

    Public Function GetParametroInt2(ByVal _nombre As String) As Integer
        Dim _result As Integer = -1

        Try
            Dim db As New SecurityDataContext(GetConnString)

            Dim _param = db.Parametros.FirstOrDefault(Function(d) d.Descripcion = _nombre)

            If Not IsNothing(_param) Then
                _result = _param.CustomInt2
            Else
                _result = String.Empty
            End If
        Catch ex As Exception
            'to do
        End Try

        Return _result
    End Function

    Public Class Enums

        Public Enum Parametros
            seguridad = 2
            tipos_categorias = 10
        End Enum

        Public Enum CategoryType
            fotosUp = 1
            videos = 2
            texto = 3
            link = 4
            contacto = 5
            fotosDown = 6
            none = 7
        End Enum

    End Class

End Module

