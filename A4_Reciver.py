import socket
try:
    import urlib
except:
    print("Error, libreria urlib no encontrada")
import threading
import time
import io
import time
import os

from SimpleXMLRPCServer import SimpleXMLRPCServer


NumTest = 25



localIP     = "192.168.40.72"

localPort   = 11000

bufferSize  = 1024



# Create a datagram socket

UDPServerSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)

 

# Bind to address and ip

UDPServerSocket.bind((localIP, localPort))


print("UDP server up and listening")


if os.path.exists('tramasPerdidas.txt'):
    print('El archivo existe y pesa:')
    size = os.path.getsize('tramasPerdidas.txt')
    print(size)
    if size > 50000:
      os.remove("tramasPerdidas.txt")
      print('El archivo se ha borrado')

    


# Listen for incoming datagrams

class client_thread(threading.Thread):
  def __init__(self, name):
    threading.Thread.__init__(self)
    self.name = name

  def run(self):

    global NumTest
    global primerEstado 
    primerEstado = 1
    global Mensaje
    Mensaje = 0
    global num_consultas 
    num_consultas = 0
    global tramas 
    tramas = 1
    global constant 
    constant = 13
    global robotR40, robotR41, robotR42, robotR43, robotR44, robotR50, robotR51, robotR52, robotR53, robotR54, robotR55, robotR56
    robotR40 = 0
    robotR41 = 0
    robotR42 = 0
    robotR43 = 0
    robotR44 = 0
    robotR50 = 0
    robotR51 = 0
    robotR52 = 0
    robotR53 = 0
    robotR54 = 0
    robotR55 = 0
    robotR56 = 0   
    global estado_comunicaciones
    estado_comunicaciones = 1
    global autoincremental
    global autoincrementalAnterior
    global contador_trames
    contador_trames = 0
    global contador_trames_comprova 
    contador_trames_comprova = 0
    
    
    


    bufferSize = 1024

    
    while True:

    
      bytesAddressPair = UDPServerSocket.recvfrom(bufferSize)
      

      message = bytesAddressPair[0]
      
      address = bytesAddressPair[1] 


      clientMsg = "Message from Client:{}".format(message)
      clientIP  = "Client IP Address:{}".format(address)
            
      print(clientMsg)
      print(clientIP)
      #print(robotR40, robotR41, robotR42, robotR43, robotR44, robotR50, robotR51, robotR52, robotR53, robotR54, robotR55, robotR56)
      

      Mensaje = clientMsg
      contador_trames += 1

      
    
      
      # comprovem quin es el valor de l'estat i el guardem.
      for i in clientMsg:
        i=20

        if clientMsg[i + 1] == ',':
          estado = clientMsg [i]
          print(estado)
          break
        elif clientMsg[i + 2] == ',':
          estado = clientMsg[i] + clientMsg[i + 1]
          print(estado)
          break
        elif clientMsg[i + 3] == ',':
          estado = clientMsg[i] + clientMsg[i + 1] + clientMsg[i + 2]
          print(estado)
          break
        elif clientMsg[i + 4] == ',':
          estado = clientMsg[i] + clientMsg[i + 1] + clientMsg[i + 2] + clientMsg[i + 3]
          print(estado)
          break
        else:
          print ("Estado: OUT OF RANGE")


      # si es el primer estat rebut, l'acceptem.
      if primerEstado == 1:
        print("Primer Estado Recivido")
        estado_comunicaciones = 1

        
      # si no es el primer estat rebut, comprovem que es correlatiu respecte a l'anterior.
      elif estadoAnterior == int(estado):
        print("Estado Correcto")
        estado_comunicaciones = 1


      elif estadoAnterior == 10000 and int(estado) == 1:
        print("Estado Correcto")
        estado_comunicaciones = 1


      # si no es correlatiu es mostra un error.
      else:
        
        estado_comunicaciones = -1
        print("Error en el estado recivido.")

        # creem l'arxiu de trames perdudes si aquest no existeix.
        my_file = open("tramasPerdidas.txt","a")
        my_file.write('\n Trama/s perdida entre: ')
        estadoAnterior -= 1
        my_file.write(str(estadoAnterior))
        my_file.write('-')
        my_file.write(str(estado))
        fecha = ". Fecha y hora " + time.strftime("%c") 
        my_file.write(fecha)
        



      # comprovem quin es el valor de l'autoincremental i el guardem.

      coma = 0
      for indice in range(len(clientMsg)):
        caracter = clientMsg[indice]
        if coma == 30:
          if clientMsg[indice + 2] == ',':
            autoincremental = clientMsg [indice + 1]
            break
          elif clientMsg[indice + 3] == ',':
            autoincremental = clientMsg [indice + 1] + clientMsg [indice + 2]
            break
          elif clientMsg[indice + 4] == ',':
            autoincremental = clientMsg [indice + 1] + clientMsg [indice + 2] + clientMsg [indice + 3]
            break
          elif clientMsg[indice + 5] == ',':
            autoincremental = clientMsg [indice + 1] + clientMsg [indice + 2] + clientMsg [indice + 3] + clientMsg [indice + 4]
            break
        elif clientMsg[indice] == ',':
          indice = indice + 1
          coma = coma + 1
        else:
          indice = indice + 1
      
      #print (autoincremental)
      


      # si es el primer autoincremental rebut, l'acceptem.
      if primerEstado == 1:
        print("Primer Autoincremental Recivido")

        
      # si no es el primer autoincremental, comprovem que es correlatiu respecte a l'anterior.
      elif int(autoincrementalAnterior)  == int(autoincremental):
        print("Autoincremental Correlatiu")


      elif autoincrementalAnterior == 10000 and int(autoincremental) == 1:
        print("Autoincremental Correlatiu")


     
      # si l'autoincremental no canvia.
      elif int(autoincremental) == int(autoincrementalAnterior)-1:
        print("Mateix incremental rebut")
        #if estado_comunicaciones != -1:
          #estado_comunicaciones = -2
        

      
      autoincrementalAnterior = int(autoincremental) + 1

      global tramas 
      tramas = estado



      coma = 0
      for indice in range(len(Mensaje)):
        caracter = Mensaje[indice + 1]
        if coma == 31:

          msg= list(Mensaje)
          msg[indice + 1] = estado_comunicaciones
          Mensaje = ''.join(str(e) for e in msg)

          break

        elif Mensaje[indice] == ',':
          indice = indice + 1
          coma = coma + 1
        else:
          indice = indice + 1
            
      
      a = str(num_consultas).encode() + ", " + "14, " + str(tramas).encode() + ", " + str(constant).encode() + ", " + str(robotR40).encode() + ", " + str(robotR41).encode() + ", " + str(robotR42).encode() + ", " + str(robotR43).encode() + ", " + str(robotR44).encode() + ", " + str(robotR50).encode() + ", " + str(robotR51).encode() + ", " + str(robotR52).encode() + ", " + str(robotR53).encode() + ", " + str(robotR54).encode() + ", " + str(robotR55).encode() + ", " + str(robotR56).encode() + ";"
      
      UDPServerSocket.sendto(a,address) 
      #UDPServerSocket.sendto(str(num_consultas).encode(), address)
          
        
      estadoAnterior = int(estado) + 1

      primerEstado += 1

      time.sleep(0.1)

      print(estado_comunicaciones)

    

# Funcio get_next_pose

def get_next_pose(p):
  assert type(p) is dict
  pose = urlib.poseToList(p)
  print("Received pose: " + str(pose))
  pose = [-0.18, -0.61, 0.23, 0, 3.12, 0.04]
  return urlib.listToPose(pose)
        
def trama2(R40, R41, R42, R43, R44, R50, R51, R52, R53, R54, R55, R56):

  global num_consultas
  if num_consultas == 9999:
    num_consultas = 1
  else:
    num_consultas += 1
  global Mensaje
  

  global robotR40, robotR41, robotR42, robotR43, robotR44, robotR50, robotR51, robotR52, robotR53, robotR54, robotR55, robotR56
  robotR40 = R40
  robotR41 = R41
  robotR42 = R42
  robotR43 = R43
  robotR44 = R44
  robotR50 = R50
  robotR51 = R51
  robotR52 = R52
  robotR53 = R53
  robotR54 = R54
  robotR55 = R55
  robotR56 = R56
  
  return num_consultas, Mensaje


def prova():
  #global tramas
  global num_consultas
  if num_consultas == 9999:
    num_consultas = 1
  else:
    num_consultas += 1
  global Mensaje
  return num_consultas, Mensaje

def trama():
  global primerEstado
  return primerEstado





def XMLRPC_service():
  
  # Connexio amb XMLRPC
  
  
  server = SimpleXMLRPCServer(("localhost", 8000), logRequests=True)
  
  server.register_introspection_functions()
  print("Listening on port 8000...")

  
  server.register_function(get_next_pose, 'get_next_pose')
  server.register_function(trama2, 'trama2')
  server.register_function(prova, 'prova')
  server.register_function(trama, 'trama')
  server.serve_forever()

class timerComprova(threading.Thread):
  def __init__(self, name):
    threading.Thread.__init__(self)
    self.name = name

  def run(self):
    global contador_trames_comprova
    global estado_comunicaciones
    valor_complert = 10
    while(True):
      start_time = time.time()
      interval = 1

      for i in range(valor_complert):
        time.sleep(start_time + i*interval - time.time())

        if contador_trames != contador_trames_comprova:
          contador_trames_comprova = contador_trames
          #estado_comunicaciones = 0
          

          #print("CORRELATIUUU CADA 1 SEGON")
        else: 
          #print("NO CORRELATIUUU CADA 1 SEGON")
          contador_trames_comprova = contador_trames
          #estado_comunicaciones = 1
  



def main(NumTest):


  th = client_thread("HH")

  print("---Starting Thread 1...")

  th.start()

  th2 = timerComprova("th2")

  th2.start()

  print("---Thread 1 started, starting XML SERVER...")

  XMLRPC_service()

  print("---Thread 2 started...")
  
    
    
if __name__ == '__main__':
  main(NumTest)

	