var SearchAndShortlist = SearchAndShortlist || {};

SearchAndShortlist.Cookie = function (name)
{
    this.Name = name;
    this.SubKeys = [];

    this.PopulateFromString = function (cookieString)
    {
        this.SubKeys = [];

        var subKeyStrings = cookieString.split("&");

        for (var index = 0; index < subKeyStrings.length; index++)
        {
            var subKey = new SearchAndShortlist.CookieSubKey();
            subKey.PopulateFromString(subKeyStrings[index]);
            this.SubKeys.push(subKey);
        }
    };

    this.AddSubKey = function (keyName)
    {
        var key = new SearchAndShortlist.CookieSubKey(keyName);
        this.SubKeys.push(key);
        return key;
    };

    this.AddSubKeyValue = function(key, value) {
        var subKey = this.SubKeys.find(function(element) {
            return element.Key === key;
        });

        if (!subKey) {
            subKey = this.AddSubKey(key);
        }

        subKey.AddValue(value);
    };

    this.RemoveSubKey = function (keyName)
    {
        var index = this.SubKeys.findIndex(function (element)
        {
            return element.Key === keyName;
        });

        if (index > -1)
        {
            this.SubKeys.splice(index, 1);
        }
    };

    this.RemoveSubKeyValue = function (key, value)
    {
        var subKey = this.SubKeys.find(function (element)
        {
            return element.Key === key;
        });

        if (subKey)
        {
            subKey.RemoveValue(value);
        }
    }

    this.ToString = function ()
    {
        var subKeyStrings = [];

        for (var index = 0; index < this.SubKeys.length; index++)
        {
            subKeyStrings.push(this.SubKeys[index].ToString());
        }

        return subKeyStrings.join("&");
    }
}