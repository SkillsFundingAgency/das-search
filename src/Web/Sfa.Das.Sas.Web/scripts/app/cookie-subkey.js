var SearchAndShortlist = SearchAndShortlist || {};

SearchAndShortlist.CookieSubKey = function(key) {
    this.Key = key;
    this.Values = [];

    this.PopulateFromString = function(keyString) {
        if (keyString && keyString.length !== 0) {
            var keypair = keyString.split("=");

            this.Key = keypair[0];

            if (keypair[1]) {
                this.Values = keypair[1].split("|");
            }
        }
    };

    this.AddValue = function(value) {
        this.Values.push(value);
    };

    this.AddValues = function(values) {
        this.Values.push.apply(this.Values, values);
    };

    this.RemoveValue = function(value) {
        var index = this.Values.indexOf(value);

        if (index > -1) {
            this.Values.splice(index, 1);
        }
    };

    this.ToString = function() {
        var objString = this.Key ? this.Key + "=" : "";

        objString += this.Values ? this.Values.join("|") : "";

        return objString;
    };
};