# AjaxDropDowns.MVC.Control

This package contains two html helper extensions to create a drop down box either filled via an ajax request or from a controller action.

## Advantages

- Cleaner views and view models. No need to have the dropdown option available in the view.
- Simple controller methods with corresponding result types. 
- Not only are the HTML elements created, but also the required JavaScripts.
- Optimization and minification of JavaScripts.
- The use and data binding is analogous to the normal DropDownBoxFor-HtmlHelper.

## Dependency

The dropdown boxes created require the jQuery library. This must be loaded before the first control. It is therefore advisable to move the loading of jQuery to the beginning of the HTML code.

## Usage

### AjaxDropDownListFor

Basically, you can use the *AjaxDropDownListFor* html helper extension as you use the *DropDownListFor*. Instead of the list of *SelectListItems* you pass in a read url. That read url usually
should point to a controller action which returns a json result with a list of *SelectListItems*. That's it.

```
@Html.AjaxDropDownListFor(model => model.Option1, Url.Action("Options", "DropDown")))
```

### ScriptDropDownListFor

The html helper extension *ScriptDropDownListFor* works very similar than the *AjaxDropDownListFor*. Actually the usage in the view is completely the same.

```
@Html.ScriptDropDownListFor(model => model.Option1, Url.Action("Options", "DropDown")))
```

The diffrence is that the result of the read url must not be a simple json result, but should be of the type *ScriptDropDownResult*. This encapsulates a certain content result with a java script array
with the option values for the drop down box. This content will be written directly in the response stream.

## Diffrence between AjaxDropDownListFor and ScriptDropDownListFor

The *AjaxDropDownListFor* gets its option values - like the name says - via an ajax request. I. e., First an empty box is rendered, then a piece of java script reads the options and puts them in
the drop down box. This works fine with fast internet connections, few options and few ajax requests per page.\
\
However, if you have lots of ajax request with slow connections, it may become noticable that the page "is loaded in pieces". That is where the *ScriptDropDownListFor* may come iin handy. Even though the
options are read from a controller action, not the user's client makes the request. The user's client still makes only one request, the html helper calls the controller action, and only one response is
delivered to the client.

## Further infomation

Please see the [homepage](http://net.rainerthiele.de/Home/Page/AjaxDropDown/Index) for further information.

## License
[MIT](https://choosealicense.com/licenses/mit/)