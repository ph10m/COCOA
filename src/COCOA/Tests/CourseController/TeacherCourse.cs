using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
