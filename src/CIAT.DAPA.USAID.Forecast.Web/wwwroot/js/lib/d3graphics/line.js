﻿/**
 * This class draws a line graphic
 * (Base) base: Configuration to render the graphic
 */
function Line(base) {
    this.base = base;

    this.circleContainer = null;
}

/*
 * Method to interpolate
*/
Line.prototype.tween= function (b, callback) {
    return function (a) {
        var i = d3.interpolateArray(a, b);

        return function (t) {
            return callback(i(t));
        };
    };
}

/*
 * 
*/
Line.prototype.render = function () {    
    var that = this;

    this.base.init(true, 0.5);
    
    // TODO code duplication check how you can avoid that
    var x = d3.time.scale().range([this.base.margin.right, this.base.width_full - this.base.margin.left]);
    var y = d3.scale.linear().range([this.base.height, 0]);

    var area = d3.svg.area()
            .interpolate('linear')
            .x(function (d) { return x(d.date); })
            .y0(this.base.height)
            .y1(function (d) { return y(d.value); });

    var line = d3.svg.line()
            .interpolate('linear')
            .x(function (d) { return x(d.date); })
            .y(function (d) { return y(d.value); });

    var startData = this.base.data.raw.map(function (datum) {
        return {
            date: datum.date,
            value: 0
        };
    });

    // Compute the minimum and maximum date, and the maximum value.
    x.domain([this.base.data.raw[0].date, this.base.data.raw[this.base.data.raw.length - 1].date]);
    // Compute the maximun value more 10%
    y.domain([0, d3.max(this.base.data.raw, function (d) { return d.value; }) * 1.1]);

    // Add the axis
    this.base.addAxis(x, y, 10);

    // Add the ticks
    this.base.addAxisTicks(x, y, this.base.data.raw.length, 12);

    // Add the line path.
    this.base.svg.append('g')
            .attr('class', 'line_area_line')
            .append('path')
            .datum(startData)            
            .attr('d', line)
            .transition()
            .duration(that.base.animation.duration)
            .delay(that.base.animation.delay)
            .attrTween('d', that.tween(this.base.data.raw, line))
            .each('end', function () {
                // Line Normal
                var line_splitted = that.base.svg.append('g')
                                    .attr('class', 'line_area_splitted')
                                    .datum(that.base.data.splitted)
                                    .append('line')
                                    .attr('x1', that.base.margin.right)
                                    .attr('y1', function (d) { return y(d); })
                                    .attr('x2', that.base.width_full - that.base.margin.left)
                                    .attr('y2', function (d) { return y(d); });
                /*
                line_splitted.selectAll('line_area_splitted')
                      .append("text")
                      .attr('dx', that.base.margin.right)
                      .attr('dy', function (d) { return y(d); })
                      .attr('class', 'pie_center_text_small')
                      .text(function (d) { return 'Normal'; });*/

                // Add circles to show more details
                var circles = that.base.svg.append('g')
                                .attr("class", "line_area_point");
                circles.selectAll('.line_area_point')
                    .data(that.base.data.raw)                    
                    .enter()  
                    .append('circle')
                    .attr('class', 'line_area_circle line_area_circle_highlighted')
                    .attr('r', 5)
                    .attr('cx', function (d) { return x(d.date); })
                    .attr('cy', function (d) { return y(d.value); });
            });

    // Add the area path.
    this.base.svg.append('g')
        .attr('class', 'line_area_area')
        .append('path')
        .datum(startData)
        .attr('d', area)
        .transition()
        .duration(that.base.animation.duration)
        .attrTween('d', that.tween(this.base.data.raw, area));
}