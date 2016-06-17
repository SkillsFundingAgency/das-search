var SearchAndShortlist = SearchAndShortlist || {};
(function (provider)
{
    provider.StandardCookieName = 'das_shortlist_standards';
    provider.FrameworkCookieName = 'das_shortlist_frameworks';

    provider.AddStandardProvider = function (providerId, apprenticeshipId, locationId)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(provider.StandardCookieName);

        if (cookie)
        {
            cookie.AddSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    provider.AddFrameworkProvider = function (providerId, apprenticeshipId, locationId) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(provider.FrameworkCookieName);

        if (cookie) {
            cookie.AddSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    provider.RemoveStandardProvider = function(providerId, apprenticeshipId, locationId) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(provider.StandardCookieName);

        if (cookie) {
            cookie.RemoveSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    provider.RemoveFrameworkProvider = function (providerId, apprenticeshipId, locationId) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(provider.FrameworkCookieName);

        if (cookie) {
            cookie.RemoveSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    provider.init = function()
    {
        $('.provider-shortlist-link').on('click', function (e)
        {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr('data-action') === 'add')
            {
                if ($(this).attr('data-apprenticeship-type') === 'Standard') {
                    provider.AddStandardProvider(
                        $(this).attr('data-provider'),
                        $(this).attr('data-apprenticeship'),
                        $(this).attr('data-location'));
                } else {
                    provider.AddFrameworkProvider(
                        $(this).attr('data-provider'),
                        $(this).attr('data-apprenticeship'),
                        $(this).attr('data-location'));
                }

                $('.provider-shortlist-link').attr('data-action', 'remove');
                $('.provider-shortlist-link').html('Remove this training provider');
            }
            else if ($this.attr('data-action') === 'remove')
            {
                if ($(this).attr('data-apprenticeship-type') === 'Standard') {
                    provider.RemoveStandardProvider(
                        $(this).attr('data-provider'),
                        $(this).attr('data-apprenticeship'),
                        $(this).attr('data-location'));
                } else {
                    provider.RemoveFrameworkProvider(
                       $(this).attr('data-provider'),
                       $(this).attr('data-apprenticeship'),
                       $(this).attr('data-location'));
                }

                $('.provider-shortlist-link').attr('data-action', 'add');
                $('.provider-shortlist-link').html('Shortlist this training provider');
            };
        });
    };

   provider.init();

}(SearchAndShortlist.provider = {}));