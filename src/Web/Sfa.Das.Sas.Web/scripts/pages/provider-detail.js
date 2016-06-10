var SearchAndShortlist = SearchAndShortlist || {};
(function (provider)
{
    provider.StandardCookieName = 'das_shortlist_standards';
    provider.FrameworkCookieName = 'das_shortlist_frameworks';

    provider.Add = function (providerId, apprenticeshipId, locationId)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(provider.StandardCookieName);

        if (cookie)
        {
            cookie.AddSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    provider.Remove = function(providerId, apprenticeshipId, locationId) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(provider.StandardCookieName);

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
                provider.Add(
                    $(this).attr('data-provider'),
                    $(this).attr('data-apprenticeship'),
                    $(this).attr('data-location'));

                $('.provider-shortlist-link').attr('data-action', 'remove');
                $('.provider-shortlist-link').html('Remove this training provider');
            }
            else if ($this.attr('data-action') === 'remove')
            {
                provider.Remove(
                    $(this).attr('data-provider'),
                    $(this).attr('data-apprenticeship'),
                    $(this).attr('data-location'));

                $('.provider-shortlist-link').attr('data-action', 'add');
                $('.provider-shortlist-link').html('Shortlist this training provider');
            };
        });
    };

   provider.init();

}(SearchAndShortlist.provider = {}));