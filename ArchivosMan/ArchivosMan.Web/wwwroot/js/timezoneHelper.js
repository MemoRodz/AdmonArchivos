window.timezoneHelper = {
    getLocalTimeZone: function () {
        return Intl.DateTimeFormat().resolvedOptions().timeZone;
    }
};