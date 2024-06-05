import zmq
from playsound import playsound
import os

def setupSocketReply(ip, port, localhost  = False):
    context = zmq.Context()
    socket = context.socket(zmq.REP)
    if localhost:
        socket.bind(f"tcp://*:{port}")
        return socket
    socket.bind(f"tcp://{ip}:{port}")
    return socket

def play_sound():
    playsound(fr"{os.getcwd()}\sound_coin.mp3")


if __name__ == "__main__":
    PORT = 5557
    ip = '172.0.0.1' # this is a standard localhost ip, change it to the ip of your

    socket = setupSocketReply(ip, PORT, localhost=False)

    while True:
        print("Waiting for a new string")
        message = socket.recv_string()
        print(f"Received: {message}")
        play_sound()
        socket.send_string("Ok!")
        print("Sent string")
    