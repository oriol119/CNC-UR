Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports System
Imports System.Runtime.InteropServices
Imports Api8070

Public Module GlobalVariables
    Public lastRecivedResp As Integer = 0
    Public array_parameters(31) As String
    Public IP_CNC As String = "127.0.0.1"
    Public estado_comunicaciones As Integer = 0
    Public R30_value
    Public text
    Public trama_respuesta_comparador As Integer = 0




End Module

Module Sender
    Dim svr2 As Api8070.CNC8070_OpcServer

    Public WithEvents variables As CNC8070_Variables
    Public WithEvents AxesTpos As CNC8070_Axes
    Public WithEvents AxesFlwe As CNC8070_Axes
    Public WithEvents Kernel As CNC8070_Kernel
    Dim keys As CNC8070_KeystrokeEngine
    Public WithEvents Jog As CNC8070_Jog
    Public WithEvents Plc As Api8070.CNC8070_Plc
    Dim ErrorCNC As Api8070.CNC8070_Error
    Public WithEvents Pparam As Api8070.CNC8070_PParamTable
    Public WithEvents Origin As CNC8070_OriginTable
    Dim i As Integer
    
    Sub Main()

        Dim A As System.Threading.Thread = New Threading.Thread(AddressOf TaskA)
        A.Start()




        If File.Exists("C:\Documents and Settings\Electric\Escritorio\prova fitxer text\provemtext.txt") Then
            Dim fileName As String = "C:\Documents and Settings\Electric\Escritorio\prova fitxer text\provemtext.txt"

            Dim fi As New IO.FileInfo(fileName)
            Dim size As Long = fi.Length


            If (size > 50000) Then
                My.Computer.FileSystem.DeleteFile("C:\Documents and Settings\Electric\Escritorio\prova fitxer text\provemtext.txt")
                MsgBox("FILE DELETED, IT WAS BIGGER THAN 50.000 bytes")

            End If
        End If


        MsgBox("Obrir CNC")


        '• Conectarse al servidor de variables.
        variables = CreateObject("cnc8070.variables", IP_CNC)
        'Stt()


        Dim pVarStatus As Integer
        pVarStatus = variables.OpenVariable("G.STATUS")
        '• Leer una variable.
        Dim textStatus As String
        textStatus = variables.ReadTxt(pVarStatus, -1)

        MsgBox(textStatus)
        temporitzador()


    End Sub

    Sub ReadValues()


        ' Conectarse al servidor de parametros aritmeticos.
        Pparam = CreateObject("cnc8070.pparamtable", IP_CNC)


        Dim pVarStatus As String


        pVarStatus = Pparam.ReadCommonPParams(9) 'Llegim el valor de P10009 
        array_parameters(0) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(8) 'Llegim el valor de P10008 
        array_parameters(1) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(6) 'Llegim el valor de P10006
        array_parameters(2) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(5) 'Llegim el valor de P10005 
        array_parameters(3) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(0) 'Llegim el valor de P10000 
        array_parameters(4) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(1) 'Llegim el valor de P10001
        array_parameters(5) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(2) 'Llegim el valor de P10002 
        array_parameters(6) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(3) 'Llegim el valor de P10003 
        array_parameters(7) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(60) 'Llegim el valor de P10060
        array_parameters(8) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(61) 'Llegim el valor de P10061
        array_parameters(9) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(62) 'Llegim el valor de P10062
        array_parameters(10) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(63) 'Llegim el valor de P10063
        array_parameters(11) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(64) 'Llegim el valor de P10064
        array_parameters(12) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(65) 'Llegim el valor de P10065
        array_parameters(13) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(7) 'Llegim el valor de P10007
        array_parameters(14) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(12) 'Llegim el valor de P10012
        array_parameters(15) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(13) 'Llegim el valor de P10013
        array_parameters(16) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(14) 'Llegim el valor de P10014
        array_parameters(17) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(15) 'Llegim el valor de P10015
        array_parameters(18) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(16) 'Llegim el valor de P10016
        array_parameters(19) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(17) 'Llegim el valor de P10017
        array_parameters(20) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(18) 'Llegim el valor de P10018
        array_parameters(21) = pVarStatus + ","
        pVarStatus = Pparam.ReadCommonPParams(19) 'Llegim el valor de P10019
        array_parameters(22) = pVarStatus + ","

        AxesTpos = CreateObject("cnc8070.Axes", IP_CNC)
        AxesTpos.ConnectToChannel(1, "PPOS")


        pVarStatus = AxesTpos.GetTxtValues(0) 'Llegim el valor de V.A.POS.X
        array_parameters(23) = pVarStatus + ","
        pVarStatus = AxesTpos.GetTxtValues(1) 'Llegim el valor de V.A.POS.Y
        array_parameters(24) = pVarStatus + ","
        pVarStatus = AxesTpos.GetTxtValues(2) 'Llegim el valor de V.A.POS.Z
        array_parameters(25) = pVarStatus + ","
        pVarStatus = AxesTpos.GetTxtValues(3) 'Llegim el valor de V.A.POS.V
        array_parameters(26) = pVarStatus + ","
        pVarStatus = AxesTpos.GetTxtValues(4) 'Llegim el valor de V.A.POS.W
        array_parameters(27) = pVarStatus + ","

        AxesTpos.DisconnectAxes()

        Dim R31, R32, R33 As Double
        Dim R31_value, R32_value, R33_value As String

        variables = CreateObject("cnc8070.variables", IP_CNC)

        'R30 = variables.OpenVariable("PLC.R[30]") 'Obrim la variable R30
        R31 = variables.OpenVariable("PLC.R[31]") 'Obrim la variable R31
        R32 = variables.OpenVariable("PLC.R[32]") 'Obrim la variable R32
        R33 = variables.OpenVariable("PLC.R[33]") 'Obrim la variable R33 

        'R30_value = variables.ReadTxt(R30, -1) 'Llegim el valor de R30
        R31_value = variables.ReadTxt(R31, -1) 'Llegim el valor de R31
        R32_value = variables.ReadTxt(R32, -1) 'Llegim el valor de R32
        R33_value = variables.ReadTxt(R33, -1) 'Llegim el valor de R33 

        array_parameters(28) = "4444" + ","  'Afegim R30 a l'array
        array_parameters(29) = "0" + ","  'Afegim R31 a l'array
        array_parameters(30) = R32_value + ","  'Afegim R32 a l'array
        array_parameters(31) = R33_value + ","  'Afegim R33 a l'array
      

    End Sub



    Function WriteUDP(ByVal mensaje, ByVal estado) As String

        Console.WriteLine("Sender")
        Console.WriteLine("Visual Basic 2005 EXPRESS .NET 2.0")

        Dim UDPClient As New UdpClient()
        UDPClient.Client.SetSocketOption(SocketOptionLevel.Socket, _
           SocketOptionName.ReuseAddress, True)
        UDPClient.Connect("192.168.40.85", 11000)
        Dim IPEpoint As New System.Net.IPEndPoint(Net.IPAddress.Parse("192.168.40.85"), 1045)




        Dim ContentToSend As String



        ContentToSend = estado & ", " & mensaje.Length & ", "

        For i = 0 To mensaje.Length - 1

            ContentToSend += mensaje(i) & " "
            If i = mensaje.Length - 1 Then
                ContentToSend += ";"
            Else
            End If
        Next

        Console.WriteLine(ContentToSend)
        Dim bytSent As Byte() = _
           Encoding.ASCII.GetBytes(ContentToSend)

        UDPClient.Send(bytSent, bytSent.Length)



        'If UDPClient.Client.ReceiveTimeout = 1000 Then
        'UDPClient.Client.Blocking = False



        text = UDPClient.Receive(IPEpoint)  'rebem la resposta
        Dim vOut As String = System.Text.Encoding.ASCII.GetString(text)





        Dim consulta_xmlrpc As String

        Console.WriteLine(vOut)

        'Llegim el numero de consultes xml-rpc (a la resposta)

        If vOut(1) = "," Then
            consulta_xmlrpc = vOut(0)
        ElseIf vOut(2) = "," Then
            consulta_xmlrpc = vOut(0) + vOut(1)
        ElseIf vOut(3) = "," Then
            consulta_xmlrpc = vOut(0) + vOut(1) + vOut(2)
        ElseIf vOut(4) = "," Then
            consulta_xmlrpc = vOut(0) + vOut(1) + vOut(2) + vOut(3)
        End If

        'Tractem el número de consultes xml-rpc


        If consulta_xmlrpc = 0 Or consulta_xmlrpc = lastRecivedResp Then
            Console.WriteLine("NINGUNA CONSULTA")
            estado_comunicaciones = 0

        ElseIf consulta_xmlrpc - 1 = lastRecivedResp Or (consulta_xmlrpc = 1 And lastRecivedResp = 9999) Then
            lastRecivedResp = consulta_xmlrpc
            Console.WriteLine("CONSULTA CORRECTA") 'lastRecivesResp guarda l'ultim valor del comptador, per poder saber si hem perdut consultes
            estado_comunicaciones = 0

        Else

            Dim FILE_NAME As String = "C:\Documents and Settings\Electric\Escritorio\prova fitxer text\provemtext.txt"
            Dim thisDate As Date
            Dim thisTime As String
            thisDate = Today
            thisTime = Date.Now.ToString("hh:mm:ss")

            Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)
            Console.WriteLine("CONSULTA/S PERDIDA/S")
            objWriter.WriteLine("Consultes perdudes entre: " & lastRecivedResp & " i " & consulta_xmlrpc & " el " & thisDate & " a las " & thisTime)
            objWriter.Close()

            lastRecivedResp = consulta_xmlrpc
            If estado_comunicaciones <> 1 Then
                estado_comunicaciones = 2
            End If


        End If



        'Llegim el numero de trama de la resposta


        Dim coma1 = 0
        Dim trama_respuesta As String
        Dim indice

        For indice = 0 To vOut.Length - 1
            If coma1 = 2 Then

                If vOut(indice + 2) = "," Then
                    trama_respuesta = vOut(indice + 1)
                    coma1 = 0
                    Exit For
                ElseIf vOut(indice + 3) = "," Then
                    trama_respuesta = vOut(indice + 1) & vOut(indice + 2)
                    coma1 = 0
                    Exit For
                ElseIf vOut(indice + 4) = "," Then
                    trama_respuesta = vOut(indice + 1) & vOut(indice + 2) & vOut(indice + 3)
                    coma1 = 0
                    Exit For
                ElseIf vOut(indice + 5) = "," Then
                    trama_respuesta = vOut(indice + 1) & vOut(indice + 2) & vOut(indice + 3) & vOut(indice + 4)
                    coma1 = 0
                    Exit For
                End If

            ElseIf vOut(indice) = "," Then
                coma1 += 1
            End If


        Next



        If trama_respuesta_comparador = 0 Then
            trama_respuesta_comparador = trama_respuesta
            Console.WriteLine("PRIMERA RESPUESTA")
            Console.WriteLine(trama_respuesta)
            estado_comunicaciones = 0


        ElseIf trama_respuesta_comparador = trama_respuesta - 1 Then
            trama_respuesta_comparador = trama_respuesta
            Console.WriteLine("RESPUESTA CORRELATIVA")
            Console.WriteLine(trama_respuesta)
            estado_comunicaciones = 0
        Else
            Console.WriteLine("RESPUESTAS PERDIDAS")
            Console.WriteLine(trama_respuesta)
            estado_comunicaciones = 1



        End If


        Dim R40, R41, R42, R43, R44, R50, R51, R52, R53, R54, R55, R56
        variables = CreateObject("cnc8070.variables", IP_CNC)

        R40 = variables.OpenVariable("PLC.R[40]")
        R41 = variables.OpenVariable("PLC.R[41]")
        R42 = variables.OpenVariable("PLC.R[42]")
        R43 = variables.OpenVariable("PLC.R[43]")
        R44 = variables.OpenVariable("PLC.R[44]")
        R50 = variables.OpenVariable("PLC.R[50]")
        R51 = variables.OpenVariable("PLC.R[51]")
        R52 = variables.OpenVariable("PLC.R[52]")
        R53 = variables.OpenVariable("PLC.R[53]")
        R54 = variables.OpenVariable("PLC.R[54]")
        R55 = variables.OpenVariable("PLC.R[55]")
        R56 = variables.OpenVariable("PLC.R[56]")


        'MsgBox(vOut.Length)
        Dim R40_value, R41_value, R42_value, R43_value, R44_value, R50_value, R51_value, R52_value, R53_value, R54_value, R55_value, R56_value As String
        Dim coma2 = 0
        For i As Integer = 0 To vOut.Length - 1

            If coma2 = 4 Then

                If vOut(i + 2) = "," Then
                    R40_value = vOut(i + 1)
                    coma2 += 1
                    i += 3
                ElseIf vOut(i + 3) = "," Then
                    R40_value = vOut(i + 1) & vOut(i + 2)
                    coma2 += 1
                    i += 4
                ElseIf vOut(i + 4) = "," Then
                    R40_value = vOut(i + 1) & vOut(i + 2) & vOut(i + 3)
                    coma2 += 1
                    i += 5
                ElseIf vOut(i + 5) = "," Then
                    R40_value = vOut(i + 1) & vOut(i + 2) & vOut(i + 3) & vOut(i + 4)
                    coma2 += 1
                    i += 6

                End If

            ElseIf coma2 = 5 Then
                R41_value = estado_comunicaciones
                coma2 += 1
                i += 3
            ElseIf coma2 = 6 Then
                R42_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 7 Then
                R43_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 8 Then
                R44_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 9 Then
                R50_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 10 Then
                R51_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 11 Then
                R52_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 12 Then
                R53_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 13 Then
                R54_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 14 Then
                R55_value = vOut(i + 1)
                coma2 += 1
                i += 3
            ElseIf coma2 = 15 Then
                R56_value = vOut(i + 1)
                coma2 += 1
                i += 3

            ElseIf vOut(i) = "," Then

                coma2 += 1
                i += 1

            Else
                i += 1

            End If
            i -= 1


        Next
        variables.WriteTxt(R40, R40_value)
        variables.WriteTxt(R41, estado_comunicaciones)
        variables.WriteTxt(R42, R42_value)
        variables.WriteTxt(R43, R43_value)
        variables.WriteTxt(R44, R44_value)
        variables.WriteTxt(R50, R50_value)
        variables.WriteTxt(R51, R51_value)
        variables.WriteTxt(R52, R52_value)
        variables.WriteTxt(R53, R53_value)
        variables.WriteTxt(R54, R54_value)
        variables.WriteTxt(R55, R55_value)
        variables.WriteTxt(R56, R56_value)

        Console.WriteLine(estado_comunicaciones)

        UDPClient.Close()





        'RESPOSTA




        Return True

    End Function


    Function temporitzador()

        Dim estado ', Finish, TotalTime
        estado = 0
        Dim autoincremental = 0
        Dim pR40, pR30

        Dim interval1 = 2
        Dim interval2 = 0.5
        Dim Start1 = Timer
        Dim Start2 = Timer

        Do While True


            If Timer >= Start1 + interval1 Then


                ReadValues()

                If estado < 9999 Then

                    estado += 1
                Else
                    estado = 1
                End If

                WriteUDP(array_parameters, estado)

                Start1 = Start1 + interval1


            ElseIf Timer >= Start2 + interval2 Then


                variables = CreateObject("cnc8070.variables", IP_CNC)

                pR40 = variables.OpenVariable("PLC.R[40]")
                pR30 = variables.OpenVariable("PLC.R[30]")

                variables.WriteTxt(pR40, autoincremental)

                R30_value = variables.ReadTxt(pR30, -1)

                autoincremental += 1
                Start2 = Start2 + interval2


            End If

        Loop

        End

        Return True
    End Function


    Sub TaskA()



    End Sub

End Module
