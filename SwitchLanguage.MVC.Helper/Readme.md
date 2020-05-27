# SwitchLanguage.MVC.Helper

This package contains an url helper extension and an action filter to make switching language in web applications easier. 

## Advantages

- Easy creation of links to switch the language.
- Posibility of storing the selected language in a persistent cookie to keep it for the next page visit.
- Reading of the language parameter from a QueryString or route value.
- Language stored in the CurrentUICulture which helps using the standard resource file mechanism.
- Simple configuration.

## Usage

First you have to add the SwitchLanguageFilter to your *GlobalFilterCollection* (usually via the *RegisterGlobalFilters* method in the *FilterConfig* class):

```
filters.Add(new SwitchLanguageFilter());
```

Then, to create a link to change the language, use the *SwitchLanguageUrl* method of the url helper extension.

```
<a href="@Url.SwitchLanguageUrl("en")\">Englisch</a>;
```

## Configuration

The *SwitchLanguageFilter* has several properties to change its behaviour:

- LanguageParameterName\
*Standard value: language*\
The name of the language parameter in the QueryString. 
- LanguageParameterType\
*Standard value: QueryString*\
A parameter that determines whether the language is to be determined using the query string or the route. 
- LanguageCookieName\
*Standard value: CurrentUICulture*\
The name of the cookie in which the language for the next request and possibly for the next session (depending on the setting of the LanguageCookieExpirationDays value)is saved. 
-LanguageCookieExpirationDays\
*Standard value: 0*\
The duration of the language cookie in days. By default, zero days are given here. I. e. the cookie is discarded at the end of the session. A higher value means that the user willfind his or her selected language when visiting the page again.


## Further infomation

Please see the [homepage](http://net.rainerthiele.de/Home/Page/SwitchLanguage/Index) for further information.

## License
[MIT](https://choosealicense.com/licenses/mit/)