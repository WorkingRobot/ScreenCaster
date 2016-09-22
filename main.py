import win32gui, win32ui, win32con, os
from win32api import GetSystemMetrics
from time import time

windowname = "missingno"
w, h = GetSystemMetrics(0), GetSystemMetrics(1)
images = 200
preimage = 0
av_sum = 0
average = 0
average_index = 0

def getaverage(num):
    global average_index, average, av_sum
    average_index += 1;
    av_sum += num
    average = av_sum / average_index

starttime = 0

hwnd = win32gui.FindWindow(None, windowname)
wDC = win32gui.GetWindowDC(hwnd)
dcObj=win32ui.CreateDCFromHandle(wDC)
starttime = time()
for i in range(images):
    cDC=dcObj.CreateCompatibleDC()
    dataBitMap = win32ui.CreateBitmap()
    dataBitMap.CreateCompatibleBitmap(dcObj, w, h)
    cDC.SelectObject(dataBitMap)
    cDC.BitBlt((0,0),(w, h) , dcObj, (0,0), win32con.SRCCOPY)
    inf = win32gui.GetCursorInfo()
    cDC.DrawIcon(inf[2],inf[1])
    dataBitMap.SaveBitmapFile(cDC, str(i+1)+".bmp")
    win32gui.DeleteObject(dataBitMap.GetHandle())
    cDC.DeleteDC()
    if i != 0:
        print(i)
        getaverage(time()-preimage)
    preimage = time()
print(time() - starttime)
print("ffmpeg -y -f image2 -r %d -i %%d.bmp -vcodec libx264 -crf 0 video.mp4" % (int(1/average)))
os.system("ffmpeg -y -f image2 -r %d -i %%d.bmp -vcodec libx264 -crf 0 video.mp4" % (int(1/average)))
dcObj.DeleteDC()

win32gui.ReleaseDC(hwnd, wDC)

print("dun")
