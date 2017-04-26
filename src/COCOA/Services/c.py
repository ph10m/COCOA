'''
Before running this script:
pip install BeautifulSoup
pip install urllib
'''
import urllib
from bs4 import BeautifulSoup
import time
import datetime

parsed = open('seedWithCourseLinks.sql','w')
_c = []
with open ('courses.txt','r') as courses:
    for c in courses:
        if not 'Thesis' in c and not 'Seminar' in c:
            _c.append(c.strip().split(' ',1))


_id = 1
_course_sql = "INSERT INTO [dbo].[Courses] ([Id], [Description], [Name], [Name1024], [Timestamp], [Coordinator]) VALUES ("
_next = "', N'"
base_url = "https://www.ntnu.edu/studies/courses/"
for item in _c:
    print item[0] + '...'
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
        req = urllib.urlopen(manager_url)
        manager_soup = BeautifulSoup(req,'lxml')
        manager_mail = manager_soup.find('a', class_="email viewInput").get('href')
        manager_mail = manager_mail.replace('mailto:','')
        course_manager = manager_str_name + ", " + manager_mail
    except Exception:
        print "couldn't find any contact or email..."
        course_manager = ''
    print course_manager
        
    # Get course title and goal
    goal = ''
    try:
        goal = soup.find('p',class_='content-learning-goal')
        goal = goal.getText()
    except Exception:
        goal = ''
        print 'something went wrong.. goal is now empty'

    course_title = ''
    if item[1] is not None:
        course_title = str(item[1])
    course_title = course_title.replace("'","")
    course_title = item[0]+' '+course_title
    goal = goal.replace("'","")
    dt = datetime.datetime.fromtimestamp(time.time()).strftime('%Y-%m-%d %H:%M:%S')
    # print dt
    sql = _course_sql + str(_id) + ", N'" + goal + "', N'" + course_title + "', " + "NULL, N'" + dt + "', N'"+ course_manager + "')"
    # print 'writing...'
    parsed.write(sql.encode('utf8') + '\n')
    _id+=1
parsed.close()
