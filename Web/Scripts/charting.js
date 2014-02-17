

var SetupChartIterationSummaryMarkers = function(chart, data) {
    chart.setGallery(cfx.Gallery.Lines);    
    PopulateIterationSummary(chart, data);
    
    var yAxis = chart.getAxisY();
    yAxis.getTitle().setText("Total " + data.UnitDescription);
    
    var xAxis = chart.getAxisX();
    xAxis.getTitle().setText("Iterations");
    
    var titles = chart.getTitles();
    var title = new cfx.TitleDockable();
    title.setText(data.Title);
    titles.add(title);
};

var PopulateIterationSummary = function(chart, data) {

    var items = [];

    $.each(data.ReportItems, function() {
        items.push({            
            Achieved: this.Achieved,
            Target: this.Target,
            Trend: this.Achieved,
            XaxisValue: this.XaxisValue,
        });
    });
    
    $.each(data.Trend, function () {
        items.push({
            Trend: this.Value,
            XaxisValue: this.XaxisValue,
        });
    });

    chart.setDataSource(items);
};

var DefineIterationSummaryChart = function(containerStr, data) {
    $("#" + containerStr).html("");
    
    if (data == null) {
        return;
    }

    var chart = new cfx.Chart();
    SetupChartIterationSummaryMarkers(chart, data);
    chart.create(document.getElementById(containerStr));
    $("#" + containerStr).data("function", SetupChartIterationSummaryMarkers);
};


var SetupChartIterationDetail = function (chart, data) {
    
    chart.setGallery(cfx.Gallery.Lines);
    chart.getAllSeries().setMarkerShape(cfx.MarkerShape.None);
    
    PopulateIterationDetail(chart, data);

    var seriesCount = chart.getSeries().getCount();
    chart.getSeries().getItem(seriesCount - 1).setGallery(cfx.Gallery.Area);
    
    var yAxis = chart.getAxisY();
    yAxis.getTitle().setText("Total " + data.UnitDesciption);
    
    var xAxis = chart.getAxisX();
    xAxis.getTitle().setText("Days");
    
    var titles = chart.getTitles();
    var title = new cfx.TitleDockable();
    title.setText(data.Title);
    titles.add(title);
};

var PopulateIterationDetail = function(chart, data) {
    
    var items = [];
    $.each(data.Items, function () {

        var item = {};

        $.each(this.Values, function() {
            var i = this;
            item[i.Key] = i.Value;            
        });
        
        item.Average = this.Average;
        items.push(item);
    });

    chart.setDataSource(items);
};

var DefineIterationDetailChart = function (containerStr, data) {
    $("#" + containerStr).html("");
    
    if (data == null) {
        return;
    }

    var chart = new cfx.Chart();
    SetupChartIterationDetail(chart, data);
    chart.create(document.getElementById(containerStr));
    $("#" + containerStr).data("function", SetupChartIterationDetail);
};