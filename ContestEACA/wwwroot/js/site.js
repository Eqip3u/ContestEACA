// Write your JavaScript code.

$(document).ready(function () {
    $(function () {
        'use strict';

        $('.navbar').bootstrapAutoHideNavbar({
            disableAutoHide: false,
            delta: 5,
            duration: 250,

        });
    });


     
    $('#fullpage').fullpage({
        anchors: ['slide1', 'slide2', 'slide3', 'slide4'],
        navigation: false,
        afterLoad: function (anchorLink, index) {
            var loadedSection = $(this);


            //using anchorLink
            if (anchorLink == 'slide2' || anchorLink == 'slide3' || anchorLink == 'slide4') {

                $(function () {
                    'use strict';

                    var autohide = $('.navbar').bootstrapAutoHideNavbar({
                        disableAutoHide: true
                    });

                    autohide.show();
                    autohide.hide();
                    autohide.setDisableAutoHide(false);
                });
            }
            if (anchorLink == 'slide1' || anchorLink == 'slide5') {

                $(function () {
                    'use strict';

                    var autohide = $('.navbar').bootstrapAutoHideNavbar({
                        disableAutoHide: true
                    });

                    autohide.show();
                    autohide.setDisableAutoHide(false);
                });
            }
        }
    });

});

