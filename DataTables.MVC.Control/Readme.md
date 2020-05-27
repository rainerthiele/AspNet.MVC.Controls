# DataTables.MVC.Control

This package contains html helper extensions to create searchable, sortable and pagable data tables filled via ajax request.

## Advantages

- Model bound data tables via html helper.
- Fast and easy setup of data tables with lots of options.
- Minimal or even no JavaScript writing required (unlike the plain usage of datatables.net).
- Optimization and minification of JavaScripts.
- No effort to develop complicated sorting, filtering or paging.
- Supports server-side processing for large amounts of data or custom functionality.


## Dependency

The data tables created require the jQuery library and the [datatables.net](https://www.datatables.net/) javascript plugin. This must be loaded before the first control. It is therefore advisable to move the loading of jQuery to the beginning of the HTML code.

## Usage

The *DataTables.MVC.Control* namespace contains the generic *AjaxDataTable* html helper extension and a couple of classes to configure the table and its columns. There are built in functions to add
normal columns, numeric colums, date/time column and link column. If this is not enough, you can even configure a java script function which gives you full control over the design of the column.

Each column can be configured to be sortable or seachable. The whole table can be configured with diffrent functionalities and layout. 

## Example

Supposed you have a model named *ArticleViewModel* and a controller action *List* which delivers an array of data corresponding this model, your table configuration could look like this:

```
@(Html.AjaxDataTable<ArticleViewModel>(
          new TableConfiguration()
          {
              AjaxReadUrl = Url.Action("List")
          })
          .Column(c => c.Id)
          .LinkColumn(c => c.Id, new LinkConfiguration()
          {
              InnerHtml = "<span>{Name}</span>",
              LinkType = LinkType.Script,
              TagType = TagType.Anchor,
              Target = "testFunction({Id})"
          })
          .NumericColumn(c => c.Price, new ColumnConfiguration()
          {
              ClassName = "text-right"
          }, new NumericConfiguration()
          {
              DecimalPlaces = 2,
              DecimalSign = ",",
              ThousandsSeperator = ".",
              ValueAppendix = " €"
          })
          .DateTimeColumn(c => c.ReleaseDate, new DateTimeConfiguration("DD.MM.YYYY"))
          .Render()
)
```

This would create a four-column-table. First column would be simply the id, second a link column with the name linked to a javascript function, third a column displaying the price in 
german format and fourth a colum to show a realease date, also in german format.

## Further infomation

Please see the [homepage](http://net.rainerthiele.de/Home/Page/AjaxDataTable/Index) for further information.

## License
[MIT](https://choosealicense.com/licenses/mit/)