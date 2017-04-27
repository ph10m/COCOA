using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using System.Web.Script.Serialization;
using COCOA.Models;



namespace COCOA.Tests.CourseController
{
    /// <summary>
    /// Tests for teacher -> course.
    /// </summary>
    [TestClass]
    public class Documents : IntegrationTest
    {
        [TestMethod]
        public async Task NewDocumentSuccess()
        {
            // Act
            await RegisterSignIn("testDocumentsUploadDocumentSuccess@ntnu.no");

            var courseCreateResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);

            byte[] bytes = new Byte[] { 1, 2, 3, 4 };
            ByteArrayContent byteContent = new ByteArrayContent(bytes);

            var response = await _client.PostAsync("/course/upload?name=TestDocument.pdf&courseId=1&description=TestingDocument", byteContent);

            // Assert
            Assert.AreEqual("OK", response.StatusCode.ToString().ToUpper());
        }


        class DocumentMeta
        {
            int id;
            string name;
            string description;
        }

        [TestMethod]
        public async Task SearchDocumentSuccess()
        {
            // Act
            await RegisterSignIn("testDocumentsSearchDocumentSuccess@ntnu.no");

            var courseCreateResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);

            byte[] bytes = new Byte[] { 1, 2, 3, 4 };
            ByteArrayContent byteContent = new ByteArrayContent(bytes);

            var aResponse = await _client.GetAsync("/course/getAssignedCourses");

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var str = await aResponse.Content.ReadAsStringAsync();
            var JSONObj = ser.Deserialize<Course[]>(str);
            var uresponse = await _client.PostAsync("/course/upload?name=testDocument.pdf&courseId=" + JSONObj[0].Id + "&description=TestingDocument", byteContent);

            var response = await _client.GetAsync("/course/documentsearch?courseid=" + JSONObj[0].Id + "&searchString=test");
            var str2 = await response.Content.ReadAsStringAsync();
            var metas = ser.Deserialize<DocumentMeta[]>(str2);
            // Assert
            Assert.AreEqual(1, metas.Length);
        }

        [TestMethod]
        public async Task SearchCourseSuccess()
        {
            // Act
            await RegisterSignIn("testDocumentsSearchDocumentSuccess@ntnu.no");

            var courseCreateResponse = await _client.PostAsync("/course/newcourse?name=Databasefag&description=Blablabla&name1024=x", null);

            var response = await _client.GetAsync("/course/coursesearch?searchstring=Database");

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var str = await response.Content.ReadAsStringAsync();
            var JSONObj = ser.Deserialize<Course[]>(str);

            // Assert
            Assert.AreEqual(1, JSONObj.Length);
        }
    }
}
