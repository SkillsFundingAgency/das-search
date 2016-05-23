/// <reference path="../app/sample.js"/>
describe("A suite", function () {
    it("contains spec with an expectation", function () {
        expect(true).toBe(true);
    });

    it("refernces the sample js",
        function() {
            expect(MyTest).not.toBe(undefined);
        });
});