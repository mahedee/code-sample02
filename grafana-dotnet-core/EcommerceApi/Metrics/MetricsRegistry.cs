using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Timer;
using App.Metrics.Gauge;

namespace EcommerceApi.Metrics
{
    /// <summary>
    /// Centralized metrics registry for tracking various API performance and business metrics
    /// Used for monitoring API usage, performance, errors, and business KPIs in Grafana dashboards
    /// </summary>
    public class MetricsRegistry
    {
        #region Counter Metrics - Track API Call Frequency
        
        // Product Read Operations
        public static CounterOptions GetByProductIdCounter => new CounterOptions()
        {
            Name = "get_product_by_id_total",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Calls,
            Tags = new MetricTags("operation", "read")
        };

        public static CounterOptions GetAllProductsCounter => new CounterOptions()
        {
            Name = "get_all_products_total",
            Context = "ProductAPI", 
            MeasurementUnit = App.Metrics.Unit.Calls,
            Tags = new MetricTags("operation", "read")
        };

        // Product Write Operations
        public static CounterOptions CreateProductCounter => new CounterOptions()
        {
            Name = "create_product_total",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Calls,
            Tags = new MetricTags("operation", "create")
        };

        public static CounterOptions UpdateProductCounter => new CounterOptions()
        {
            Name = "update_product_total", 
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Calls,
            Tags = new MetricTags("operation", "update")
        };

        public static CounterOptions DeleteProductCounter => new CounterOptions()
        {
            Name = "delete_product_total",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Calls,
            Tags = new MetricTags("operation", "delete")
        };

        // Error Counters
        public static CounterOptions ProductNotFoundCounter => new CounterOptions()
        {
            Name = "product_not_found_total",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Errors,
            Tags = new MetricTags("error_type", "not_found")
        };

        public static CounterOptions ValidationErrorCounter => new CounterOptions()
        {
            Name = "validation_error_total",
            Context = "ProductAPI", 
            MeasurementUnit = App.Metrics.Unit.Errors,
            Tags = new MetricTags("error_type", "validation")
        };

        public static CounterOptions ServerErrorCounter => new CounterOptions()
        {
            Name = "server_error_total",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Errors,
            Tags = new MetricTags("error_type", "server_error")
        };

        #endregion

        #region Timer Metrics - Track Response Time Performance

        // API Response Time Timers
        public static TimerOptions GetProductByIdTimer => new TimerOptions()
        {
            Name = "get_product_by_id_duration",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Seconds
        };

        public static TimerOptions GetAllProductsTimer => new TimerOptions()
        {
            Name = "get_all_products_duration",
            Context = "ProductAPI", 
            MeasurementUnit = App.Metrics.Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Seconds
        };

        public static TimerOptions CreateProductTimer => new TimerOptions()
        {
            Name = "create_product_duration",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Seconds
        };

        public static TimerOptions UpdateProductTimer => new TimerOptions()
        {
            Name = "update_product_duration", 
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Seconds
        };

        public static TimerOptions DeleteProductTimer => new TimerOptions()
        {
            Name = "delete_product_duration",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Seconds
        };

        #endregion

        #region Histogram Metrics - Track Distribution of Values

        // Request Size Distribution
        public static HistogramOptions RequestSizeHistogram => new HistogramOptions()
        {
            Name = "request_size_bytes",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Bytes
        };

        // Response Size Distribution  
        public static HistogramOptions ResponseSizeHistogram => new HistogramOptions()
        {
            Name = "response_size_bytes",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Bytes
        };

        #endregion

        #region Gauge Metrics - Track Current State Values

        // Current System State
        public static GaugeOptions ActiveConnectionsGauge => new GaugeOptions()
        {
            Name = "active_connections_current",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Connections
        };

        public static GaugeOptions TotalProductsGauge => new GaugeOptions()
        {
            Name = "total_products_current",
            Context = "ProductAPI",
            MeasurementUnit = App.Metrics.Unit.Items
        };

        public static GaugeOptions MemoryUsageGauge => new GaugeOptions()
        {
            Name = "memory_usage_bytes",
            Context = "ProductAPI", 
            MeasurementUnit = App.Metrics.Unit.Bytes
        };

        #endregion

        #region Business Metrics - Track Business KPIs

        // Business Intelligence Counters
        public static CounterOptions ProductViewsCounter => new CounterOptions()
        {
            Name = "product_views_total",
            Context = "BusinessMetrics",
            MeasurementUnit = App.Metrics.Unit.Events,
            Tags = new MetricTags("metric_type", "engagement")
        };

        public static CounterOptions ProductCreatedTodayCounter => new CounterOptions()
        {
            Name = "products_created_today_total",
            Context = "BusinessMetrics",
            MeasurementUnit = App.Metrics.Unit.Items,
            Tags = new MetricTags("time_period", "daily")
        };

        // Average Product Price Gauge
        public static GaugeOptions AverageProductPriceGauge => new GaugeOptions()
        {
            Name = "average_product_price",
            Context = "BusinessMetrics", 
            MeasurementUnit = App.Metrics.Unit.Custom("currency")
        };

        #endregion

        #region Health Check Metrics

        // Database Health
        public static GaugeOptions DatabaseHealthGauge => new GaugeOptions()
        {
            Name = "database_health_status",
            Context = "HealthCheck",
            MeasurementUnit = App.Metrics.Unit.Custom("status")
        };

        // API Health Score
        public static GaugeOptions ApiHealthScoreGauge => new GaugeOptions()
        {
            Name = "api_health_score",
            Context = "HealthCheck",
            MeasurementUnit = App.Metrics.Unit.Percent
        };

        #endregion
    }
}