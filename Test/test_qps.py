
import threading, time, requests, random

SERVER_NAME = "http://127.0.0.1:8080"
URL = SERVER_NAME+"/user/list"
#URL = "http://172.81.205.27/"

COUNT_THREAD = 100
COUNT_REQUEST = 100

class RequestThread(threading.Thread):
    # 构造函数
    def __init__(self, thread_name):
        threading.Thread.__init__(self)
        self.count_success = 0
        self.count_fail = 0

    def run(self):
        i = 0
        while i < COUNT_REQUEST:
            self.test_performace()
            i += 1

        while True:
            if self.count_success + self.count_fail == COUNT_REQUEST:
                break

    def test_performace(self):
        try:
            r = requests.get(url=URL, timeout=30)
            self.count_success += 1
        except Exception as e:
            self.count_fail += 1
  

# 模拟用户数
threads = []
i = 0
while i < COUNT_THREAD:
    t = RequestThread("thread" + str(i))
    threads.append(t)
    t.start()
    i += 1

# 数据统计
print("concurrency:%s requests：%s\n"%(COUNT_THREAD, COUNT_THREAD*COUNT_REQUEST))
start_time = time.time()
while True:
    count = 0
    for t in threads:
        if not t.is_alive():
            count += 1
    
    # 测试结束
    if count == len(threads):
        time_span = time.time() - start_time
        count_success = 0
        count_fail = 0
        for t in threads:
            count_success += t.count_success
            count_fail += t.count_fail
        total = count_success + count_fail
        print("Request finished:\t%s"%(total))
        print("Total time:\t\t%0.3fs"%(time_span))
        print("Request success:\t%s"%(count_success))
        print("Request failed:\t\t%s"%(count_fail))
        print("Request success rate:\t%d%%"%(100*count_success/total))
        print("Request per second:\t%d"%(count_success/time_span))
        break

    time.sleep(1)

