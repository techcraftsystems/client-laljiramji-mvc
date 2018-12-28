/*
 * Dashboard - Analytics
 */

	// Polor chart data
	var doughnutData = [{
		value: 3000,
		color: "#F7464A",
		highlight: "#FF5A5E",
		label: "Diesel"
	}, {
		value: 500,
		color: "#46BFBD",
		highlight: "#5AD3D1",
		label: "Kerosene"
	}, {
		value: 1000,
		color: "#FDB45C",
		highlight: "#FFC870",
		label: "Super"
	}, {
        value: 1000,
        color: "#ab47bc",
        highlight: "#ab47bc",
        label: "V-Power"
    }];


	// Trending Bar Chart	
	var dataBarChart = {
		labels: ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug"],
		datasets: [{
			label: "Bar dataset",
			fillColor: "#46BFBD",
			strokeColor: "#46BFBD",
			highlightFill: "rgba(70, 191, 189, 0.4)",
			highlightStroke: "rgba(70, 191, 189, 0.9)",
			data: [6, 9, 8, 4, 6, 7, 9, 4]
		}]
	};

	var nReloads1 = 0;
	var min1 = 1;
	var max1 = 10;
	var l1 = 0;
	var trendingBarChart;

	function updateBarChart() {
		if (typeof trendingBarChart != "undefined") {
			nReloads1++;
			var x = Math.floor(Math.random() * (max1 - min1 + 1)) + min1;
			trendingBarChart.addData([x], dataBarChart.labels[l1]);
			trendingBarChart.removeData();
			l1++;
			if (l1 == dataBarChart.labels.length) {
				l1 = 0;
			}
		}
	}
	setInterval(updateBarChart, 5000);


	// Trending Bar chart	data
	var radarChartData = {
		labels: ["Diesel", "Super", "VPower", "Kero", "Others"],
		datasets: [{
			label: "First dataset",
			fillColor: "rgba(255,255,255,0.2)",
			strokeColor: "#fff",
			pointColor: "#00796b",
			pointStrokeColor: "#fff",
			pointHighlightFill: "#fff",
			pointHighlightStroke: "#fff",
			data: [5, 6, 7, 8, 6]
		}],
	};


	var nReloads2 = 0;
	var min2 = 1;
	var max2 = 10;
	var l2 = 0;
	var trendingRadarChart;

	function trendingRadarChartupdate() {
		if (typeof trendingRadarChart != "undefined") {
			nReloads2++;
			var x = Math.floor(Math.random() * (max2 - min2 + 1)) + min2;
			trendingRadarChart.addData([x], radarChartData.labels[l2]);
			var y = trendingRadarChart.removeData();
			l2++;
			if (l2 == radarChartData.labels.length) {
				l2 = 0;
			}
		}
	}
	setInterval(trendingRadarChartupdate, 5000);


	//Pie chart data 	
	var pieData = [{
			value: 300,
			color: "#F7464A",
			highlight: "#FF5A5E",
			label: "America"
		},
		{
			value: 50,
			color: "#46BFBD",
			highlight: "#5AD3D1",
			label: "Canada"
		},
		{
			value: 100,
			color: "#FDB45C",
			highlight: "#FFC870",
			label: "UK"
		},
		{
			value: 40,
			color: "#949FB1",
			highlight: "#A8B3C5",
			label: "Europe"
		},
		{
			value: 120,
			color: "#4D5360",
			highlight: "#616774",
			label: "Australia"
		}

	];

	// Line Chart Data	
	var lineChartData = {
		labels: ["AGO", "1K", "PMS", "VPWR", "LB", "GAS"],
		datasets: [{
			label: "My dataset",
			fillColor: "rgba(255,255,255,0)",
			strokeColor: "#fff",
			pointColor: "#00796b ",
			pointStrokeColor: "#fff",
			pointHighlightFill: "#fff",
			pointHighlightStroke: "rgba(220,220,220,1)",
			data: [65, 45, 50, 30, 63, 45]
		}]

	}

	var polarData = [{
			value: 4800,
			color: "#f44336",
			highlight: "#FF5A5E",
			label: "USA"
		},
		{
			value: 6000,
			color: "#9c27b0",
			highlight: "#ce93d8",
			label: "UK"
		},
		{
			value: 1800,
			color: "#3f51b5",
			highlight: "#7986cb",
			label: "Canada"
		},
		{
			value: 4000,
			color: "#2196f3 ",
			highlight: "#90caf9",
			label: "Austrelia"
		},
		{
			value: 5500,
			color: "#ff9800",
			highlight: "#ffb74d",
			label: "India"
		},
		{
			value: 2100,
			color: "#009688",
			highlight: "#80cbc4",
			label: "Brazil"
		},
		{
			value: 3500,
			color: "#4caf50",
			highlight: "#81c784",
			label: "Germany"
		}
	];

$(window).on('load', function() {	

	/*
	 * Reventu card
	 */
	// Trending Line chart  - use var = data  
	var trendingLineChart = document.getElementById("trending-line-chart").getContext("2d");
	window.trendingLineChart = new Chart(trendingLineChart).Line(data, {
		scaleShowGridLines: true,
		scaleGridLineColor: "rgba(255,255,255,0.4)",
		scaleGridLineWidth: 1,
		scaleShowHorizontalLines: true,
		scaleShowVerticalLines: false,
		bezierCurve: true,
		bezierCurveTension: 0.4,
		pointDot: true,
		pointDotRadius: 5,
		pointDotStrokeWidth: 2,
		pointHitDetectionRadius: 20,
		datasetStroke: true,
		datasetStrokeWidth: 3,
		datasetFill: true,
		animationSteps: 15,
		animationEasing: "easeOutQuart",
		tooltipTitleFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
		scaleFontSize: 12,
		scaleFontStyle: "normal",
		scaleFontColor: "#fff",
		tooltipEvents: ["mousemove", "touchstart", "touchmove"],
		tooltipFillColor: "rgba(255,255,255,0.8)",
		tooltipTitleFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
		tooltipFontSize: 12,
		tooltipFontColor: "#000",
		tooltipTitleFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
		tooltipTitleFontSize: 14,
		tooltipTitleFontStyle: "bold",
		tooltipTitleFontColor: "#000",
		tooltipYPadding: 8,
		tooltipXPadding: 16,
		tooltipCaretSize: 10,
		tooltipCornerRadius: 6,
		tooltipXOffset: 10,
		responsive: true
	});
	// Doughnut Chart - use data var = doughnutData  
	var doughnutChart = document.getElementById("doughnut-chart").getContext("2d");
	window.myDoughnut = new Chart(doughnutChart).Doughnut(doughnutData, {
		segmentStrokeColor: "#fff",
		tooltipTitleFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif", // String - Tooltip title font declaration for the scale label		
		percentageInnerCutout: 50,
		animationSteps: 15,
		segmentStrokeWidth: 4,
		animateScale: true,
		percentageInnerCutout: 60,
		responsive: true
	});
	// Trending Bar chart  - use data var = dataBarChart
	var trendingBarChart = document.getElementById("trending-bar-chart").getContext("2d");
	window.trendingBarChart = new Chart(trendingBarChart).Bar(dataBarChart, {
		scaleShowGridLines: false, ///Boolean - Whether grid lines are shown across the chart
		showScale: true,
		animationSteps: 15,
		tooltipTitleFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif", // String - Tooltip title font declaration for the scale label		
		responsive: true
	});

	/*
	 * Browser stats & revenue by country card
	 */
	// Trending Radar
	window.trendingRadarChart = new Chart(document.getElementById("trending-radar-chart").getContext("2d")).Radar(radarChartData, {
		angleLineColor: "rgba(255,255,255,0.5)", //String - Colour of the angle line		    
		pointLabelFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif", // String - Tooltip title font declaration for the scale label	
		pointLabelFontColor: "#fff", //String - Point label font colour
		pointDotRadius: 4,
		animationSteps: 15,
		pointDotStrokeWidth: 2,
		pointLabelFontSize: 12,
		responsive: true
	});
	// Line Chart = use data var lineChartData
	var lineChart = document.getElementById("line-chart").getContext("2d");
	window.lineChart = new Chart(lineChart).Line(lineChartData, {
		scaleShowGridLines: false,
		bezierCurve: false,
		scaleFontSize: 12,
		scaleFontStyle: "normal",
		scaleFontColor: "#fff",
		responsive: true,
	});
});



