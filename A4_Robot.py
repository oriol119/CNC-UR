import xmlrpclib
import sys
try:
    import urlib
except:
    print("Error, libreria urlib no encontrada")



print("Holaaa")
s = xmlrpclib.ServerProxy('http://localhost:8000') 

while(True):
    

    #print(s.prova())

    
    R40 = 1234
    R41 = 1
    R42 = 2
    R43 = 3
    R44 = 4
    R50 = 5
    R51 = 6
    R52 = 7
    R53 = 8
    R54 = 9
    R55 = 1
    R56 = 2
    
    print(s.trama2(R40, R41, R42, R43, R44, R50, R51, R52, R53, R54, R55, R56))
    #robot_update



    #print(s.suma())

    #pose = [2, 4, 6, 0, 8, 0]
    #print (urlib.listToPose(pose))






    

        





