using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using COCOA.Models;

namespace COCOA.Tests.CourseController
{
    /// <summary>
    /// Tests for teacher -> course.
    /// </summary>
    [TestClass]
    public class TeacherCourse : IntegrationTest
    {
        [TestMethod]
        public async Task NewCourseSuccess()
        {
            // Act
            await RegisterSignIn("testTeacherCourseNewCourseSuccess@ntnu.no");

            var response = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);

            // Assert
            Assert.AreEqual("OK", response.StatusCode.ToString().ToUpper());
        }

        [TestMethod]
        public async Task GetAssignmentsSuccess()
        {
            // Act
            await RegisterSignIn("testTeacherCourseGetAssignmentsSuccess@ntnu.no");

            var uResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);
            var response = await _client.PostAsync("/course/getAssignedCourses", null);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var JSONObj = ser.Deserialize<Course[]>(await response.Content.ReadAsStringAsync());
            // Assert
            Assert.AreEqual(1, JSONObj.Length);
        }

        [TestMethod]
        public async Task GetEnrollmentsSuccess()
        {
            // Act
            await RegisterSignIn("testTeacherCourseGetEnrollmentsSuccess@ntnu.no");

            var uResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);

            var pResponse = await _client.PostAsync("/course/getAssignedCourses", null);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var JSONObj = ser.Deserialize<Course[]>(await pResponse.Content.ReadAsStringAsync());

            var aResponse = await _client.PostAsync("/course/enrolltocourse?id=" + JSONObj[0].Id, null);

            var response = await _client.PostAsync("/course/getEnrolledCourses", null);

            var enrolls = ser.Deserialize<Course[]>(await response.Content.ReadAsStringAsync());
            // Assert
            Assert.AreEqual(1, enrolls.Length);
        }

        [TestMethod]
        public async Task CreateBulletinSuccess()
        {
            // Act
            await RegisterSignIn("testTeacherCourseNewBulletinSuccess@ntnu.no");

            var uResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);
            var aresponse = await _client.PostAsync("/course/getAssignedCourses", null);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var JSONObj = ser.Deserialize<Course[]>(await aresponse.Content.ReadAsStringAsync());
            var response = await _client.PostAsync("/course/newbulletin?courseId=" + JSONObj[0].Id + "&title=SomeTitle&content=SomeContent&href=http%3A%2F%2Fwww.ntnu.no&bulletintype=" + BulletinType.Info + "&stickey=false", null);

            // Assert
            Assert.AreEqual("OK", response.StatusCode.ToString().ToUpper());
        }

        [TestMethod]
        public async Task EnrollToCourseSuccess()
        {
            // Act
            await RegisterSignIn("testTeacherCourseEnrollSuccess@ntnu.no");

            var uResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);
            var aresponse = await _client.PostAsync("/course/getAssignedCourses", null);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var JSONObj = ser.Deserialize<Course[]>(await aresponse.Content.ReadAsStringAsync());
            var response = await _client.PostAsync("/course/enrolltocourse?id=" + JSONObj[0].Id, null);

            // Assert
            Assert.AreEqual("OK", response.StatusCode.ToString().ToUpper());
        }
    }
}
