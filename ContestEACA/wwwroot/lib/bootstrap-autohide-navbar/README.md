# bootstrap-autohide-navbar
A small jQuery plugin to show/hide twitter bootstrap 3 navbar on scroll.

### Install using bower or npm

```bash
$ bower install bootstrap-autohide-navbar
```

```bash
$ npm install bootstrap-autohide-navbar
```

### Include jQuery and Bootstrap

```markup
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.x.x/css/bootstrap.min.css">
<script src="//cdnjs.cloudflare.com/ajax/libs/jquery/1.x.x/jquery.min.js"></script>
<script src="//cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.x.x/js/bootstrap.min.js"></script>
```

### Include bootstrap-autohide-navbar.js

```html
<script src="dist/bootstrap-autohide-navbar.min.js"></script>
```

### Usage

```javascript
$(function () {
  'use strict';

  $('.navbar').bootstrapAutoHideNavbar({
    disableAutoHide: false,
    delta: 5,
    duration: 250,
    shadow: true
  });
});
```

#### or

```javascript
$(function () {
  'use strict';

  var autohide = $('.navbar').bootstrapAutoHideNavbar({
    disableAutoHide: true
  });

  autohide.show();
  autohide.hide();
  autohide.setDisableAutoHide(false);
});
```

## Options

| Property  | Required | Default | Description |
| ------------- | ------------- | ------------- | ------------- |
| delta | No | 5 | How many pixels the user needs to scroll down before effect get trigger |
| duration | No | 250 | Duration of animation in milliseconds |
| shadow | No | false | Add bottom shadow to navbar |
| disableAutoHide | No | false | Disable auto hide effect on scroll |

## Methods

| Name | Parameter | Description |
| ------------- | ------------- | ------------- |
| show | None | Show navbar |
| hide | None | Hide navbar |
| setDisableAutoHide | boolean | Disable/Enable auto hide effect |
