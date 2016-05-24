/// <reference path="../app/validation.js"/>

describe("Postcode validation", function () {
    var runValidation = function(postcode) {
        expect(SearchAndShortlist.validation.ValidatePostcode(postcode)).toBe(true, "For postcode: " + postcode);
    }

    var runNotValidation = function (postcode) {
        expect(SearchAndShortlist.validation.ValidatePostcode(postcode)).not.toBe(true, "For postcode: " + postcode);
    }

    it("Validates correct postcodes", function () {
        runValidation("A9 9AA");
        runValidation("A9A 9AA");
        runValidation("A99 9AA");
        runValidation("AA9 9AA");
        runValidation("AA9A 9AA");
        runValidation("NW67XT");
        runValidation("W1Y 8HE");
        runValidation("B721HE");
    });

    it("Validates postcodes not correct", function () {
        runNotValidation("");
        runNotValidation("YO31 1");
        runNotValidation("gfdsdf");
        runNotValidation("A9 9AAAAA");
        runNotValidation("A9 9AAAAA");
        runNotValidation("9A9 9AA");
    });

});