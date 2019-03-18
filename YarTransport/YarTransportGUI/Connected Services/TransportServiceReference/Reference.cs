﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YarTransportGUI.TransportServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TransportServiceReference.ITransportService")]
    public interface ITransportService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransportService/GetRoutes", ReplyAction="http://tempuri.org/ITransportService/GetRoutesResponse")]
        SearchWaySystem.RouteInfo[] GetRoutes(string pointOfDeparture, string pointOfDestination, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransportService/GetRoutes", ReplyAction="http://tempuri.org/ITransportService/GetRoutesResponse")]
        System.Threading.Tasks.Task<SearchWaySystem.RouteInfo[]> GetRoutesAsync(string pointOfDeparture, string pointOfDestination, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransportService/GetStations", ReplyAction="http://tempuri.org/ITransportService/GetStationsResponse")]
        string[] GetStations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransportService/GetStations", ReplyAction="http://tempuri.org/ITransportService/GetStationsResponse")]
        System.Threading.Tasks.Task<string[]> GetStationsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITransportServiceChannel : YarTransportGUI.TransportServiceReference.ITransportService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TransportServiceClient : System.ServiceModel.ClientBase<YarTransportGUI.TransportServiceReference.ITransportService>, YarTransportGUI.TransportServiceReference.ITransportService {
        
        public TransportServiceClient() {
        }
        
        public TransportServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TransportServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransportServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransportServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SearchWaySystem.RouteInfo[] GetRoutes(string pointOfDeparture, string pointOfDestination, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked) {
            return base.Channel.GetRoutes(pointOfDeparture, pointOfDestination, isBusChecked, isTrolleyChecked, isTramChecked, isMiniBusChecked);
        }
        
        public System.Threading.Tasks.Task<SearchWaySystem.RouteInfo[]> GetRoutesAsync(string pointOfDeparture, string pointOfDestination, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked) {
            return base.Channel.GetRoutesAsync(pointOfDeparture, pointOfDestination, isBusChecked, isTrolleyChecked, isTramChecked, isMiniBusChecked);
        }
        
        public string[] GetStations() {
            return base.Channel.GetStations();
        }
        
        public System.Threading.Tasks.Task<string[]> GetStationsAsync() {
            return base.Channel.GetStationsAsync();
        }
    }
}
