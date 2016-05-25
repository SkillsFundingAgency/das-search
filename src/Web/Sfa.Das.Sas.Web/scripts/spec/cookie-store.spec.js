/// <reference path="../app/cookie-store.js"/>


describe("Cookie Store Unit tests",
    function () {
    it("Should save sub keys correctly",
        function () {
        
            // Assign
            var cookie = new SearchAndShortlist.CookieStore.Cookie("test");

            // Act
            cookie.AddSubKey("1");
            var cookieString = cookie.ToString();
        
            // Assert
            expect(cookieString).toBe("1=");
        });

    it("Should save sub key value correctly",
        function () {

            // Assign
            var cookie = new SearchAndShortlist.CookieStore.Cookie("test");

            // Act
            cookie.AddSubKeyValue("1", "A");
            var cookieString = cookie.ToString();

            // Assert
            expect(cookieString).toBe("1=A");
    });

    it("Should save multiple sub key values correctly",
        function () {

            //Assign
            var cookie = new SearchAndShortlist.CookieStore.Cookie("test");

            // Act
            cookie.AddSubKeyValue("1", "A");
            cookie.AddSubKeyValue("1", "B");
            cookie.AddSubKeyValue("1", "C");
            var cookieString = cookie.ToString();

            // Assert
            expect(cookieString).toBe("1=A|B|C");
    });

    it("Should save multiple sub keys and values correctly",
        function() {

            // Assign
            var cookie = new SearchAndShortlist.CookieStore.Cookie("test");

            // Act
            cookie.AddSubKeyValue("1", "A");
            cookie.AddSubKeyValue("2", "B");
            cookie.AddSubKeyValue("2", "C");
            var cookieString = cookie.ToString();

            // Assert
            expect(cookieString).toBe("1=A&2=B|C");
        });

    it("Should not save duplicate sub keys if sub key is created already",
        function () {
            // Assign
            var cookie = new SearchAndShortlist.CookieStore.Cookie("test");

            //Act
            cookie.AddSubKey("1");
            cookie.AddSubKeyValue("1", "A");
            var cookieString = cookie.ToString();

            //Assert
            expect(cookieString).toBe("1=A");
        });

    it("Should remove sub key value",
       function () {
           // Assign
           var cookie = new SearchAndShortlist.CookieStore.Cookie("test");
           cookie.AddSubKey("1");
           cookie.AddSubKeyValue("1", "A");
           cookie.AddSubKeyValue("1", "B");
           cookie.AddSubKeyValue("2", "C");

           //Act
           cookie.RemoveSubKeyValue("1", "A");
           var cookieString = cookie.ToString();

           //Assert
           expect(cookieString).toBe("1=B&2=C");
       });

    it("Should remove only selected sub key value",
   function () {
       // Assign
       var cookie = new SearchAndShortlist.CookieStore.Cookie("test");
       cookie.AddSubKey("1");
       cookie.AddSubKeyValue("1", "A");
       cookie.AddSubKeyValue("1", "B");
       cookie.AddSubKeyValue("2", "A");

       //Act
       cookie.RemoveSubKeyValue("1", "A");
       var cookieString = cookie.ToString();

       //Assert
       expect(cookieString).toBe("1=B&2=A");
   });

    it("Should remove sub key and all its values",
   function () {
       // Assign
       var cookie = new SearchAndShortlist.CookieStore.Cookie("test");
       cookie.AddSubKey("1");
       cookie.AddSubKeyValue("1", "A");
       cookie.AddSubKeyValue("1", "B");
       cookie.AddSubKeyValue("2", "C");

       //Act
       cookie.RemoveSubKey("1");
       var cookieString = cookie.ToString();

       //Assert
       expect(cookieString).toBe("2=C");
   });




});