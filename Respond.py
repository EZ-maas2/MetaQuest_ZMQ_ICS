import zmq

def setupSocketReply(ip, port):
    context = zmq.Context()
    socket = context.socket(zmq.REP)
    socket.connect(f"tcp://{ip}:{port}")
    return socket

if __name__ == "__main__":
    PORT = 5556
    ip = '10.158.102.193'
    topic = 'Coin'
    socket = setupSocketReply(ip, PORT)

    while True:
        message = socket.recv()
        print(message)
        socket.send(1)
    