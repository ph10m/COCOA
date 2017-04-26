'''
Before running this script:
pip install BeautifulSoup
pip install urllib
'''
import urllib
from bs4 import BeautifulSoup
import time
import datetime
import re

parsed = open('seedWithCourseLinks.sql','w')
_c = []
with open ('courses.txt','r') as courses:
    for c in courses:
        if not 'Thesis' in c and not 'Seminar' in c:
            _c.append(c.strip().split(' ',1))


_id = 1
_course_sql = "INSERT INTO [dbo].[Courses] ([Id], [Description], [Name], [Name1024], [Timestamp], [Coordinator], [Infolink]) VALUES ("
_next = "', N'"
base_url = "https://www.ntnu.edu/studies/courses/"
bing_base = "https://www.bing.com/search?q="
for item in _c:
    # print item[0] + '...'
    print 'Opening course page... '+base_url+item[0]
    page = urllib.urlopen(base_url+item[0])
    soup = BeautifulSoup(page, "lxml")
    # Get course manager
    manager_mail = ''
    manager_str_name = ''
    course_manager = ''
    try:
        manager = soup.find('ul', class_="person-list nolist")
        manager_url = str(manager).split('href="')[1].split('">')[0]
        manager_name = manager_url.rsplit('/',1)[1].split('.')
        manager_str_name = ' '.join(x.title() for x in manager_name)
        print 'Finding Coordinator... '+manager_url
        req = urllib.urlopen(manager_url)
        manager_mail = BeautifulSoup(req,'lxml').find('a', class_="email viewInput").get('href')
        manager_mail = manager_mail.replace('mailto:','')
        course_manager = manager_str_name + ", " + manager_mail
    except Exception:
        print "Couldn't find any contact or email..."
        course_manager = ''
    print course_manager
        
    '''
    Get course title and goal
    '''
    goal = ''
    try:
        goal = soup.find('p',class_='content-learning-goal')
        goal = goal.getText()
    except Exception:
        goal = ''
        print 'something went wrong.. goal is now empty'
    goal = goal.replace("'","")

    course_title = ''
    if item[1] is not None:
        course_title = str(item[1])
    course_title = course_title.replace("'","")
    course_title = item[0]+' '+course_title
    
    '''
    Search bing with respect to Wikipedia links and return it
    '''
    print 'Looking for relevant information... '+bing_base+item[1]
    bing_url = urllib.urlopen(bing_base + item[1])
    bing = BeautifulSoup(bing_url,'lxml')
    # print bing.prettify().encode('utf-8')
    info_link = None
    try:
        info_link = bing.find('a', href = re.compile('^https://en.wikipedia.org/wiki/')).get('href')
    except Exception:
        print "No wikipedia results... trying another provider"
        info_link = bing.find('a', href = re.compile('^https://')).get('href')
        print info_link
    print 'FINAL LINK:',info_link

    
    dt = datetime.datetime.fromtimestamp(time.time()).strftime('%Y-%m-%d %H:%M:%S')
    # print dt
    sql = _course_sql + str(_id) + ", N'" + goal + "', N'" + course_title + "', " + "NULL, N'" + dt + "', N'"+ course_manager + "', N'" + info_link + "')"
    print 'Writing SQL statement'
    parsed.write(sql.encode('utf8') + '\n')
    _id+=1
parsed.close()
