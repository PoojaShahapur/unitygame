# -*- coding: utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
@brief start function 
'''

def main():
    import sys
    from CPP2CSharp.FrameWork import AppFrame
    app = AppFrame.AppFrame()
    sys.exit(app.exec_())
    
if __name__ == "__main__":
    main()


