'''
Created on 

@author: Administrator
'''


class MQueue():
    def __init__(self, size):
        self.size = size;
        self.front = -1;
        self.rear = -1;
        self.queue = [];
    
    
    def enqueue(self, ele):  # 入队操作
        if self.isfull():
            raise Exception("queue is full");
        else:
            self.queue.append(ele);
            self.rear = self.rear + 1;
    
    
    def dequeue(self):  # 出队操作
        if self.isempty():
            raise Exception("queue is empty");
        else:
            self.front = self.front + 1;
            return self.queue[self.front];
        
        
    def isfull(self):
        return self.rear - self.front + 1 == self.size;
    
    
    def isempty(self):
        return self.front == self.rear;
 
