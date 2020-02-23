
import threading, time, requests, random

SERVER_NAME = "http://127.0.0.1:8080"
URL = SERVER_NAME+"/user/list"
#URL = "http://172.81.205.27/"
#URL = "http://172.81.205.27:1206/"

class RequestThread(threading.Thread):
    # 构造函数
    def __init__(self, thread_name):
        threading.Thread.__init__(self)
        self.count_success = 0
        self.count_fail = 0

    def run(self):
        while True:
            self.test_performace()
            time.sleep(0.2)

    def test_performace(self):
        try:
            r = requests.get(url=URL)
            self.count_success = self.count_success + 1
        except Exception as e:
            self.count_fail = self.count_fail + 1
  

threads = []
thread_count = 100

i = 0
while i < thread_count:
    t = RequestThread("thread" + str(i))
    threads.append(t)
    t.start()
    i += 1

# 数据统计
start_time = time.time()
count_success = 0
count_fail = 0
while True:
    time_span = time.time() - start_time
    count1 = 0
    count2 = 0
    for t in threads:
        count1 += t.count_success
        count2 += t.count_fail
        
    print("%s rps success:%s fail:%s" % (count1+count2-count_success-count_fail, count1 - count_success, count2-count_fail))
    count_success = count1
    count_fail = count2

    time.sleep(1)

