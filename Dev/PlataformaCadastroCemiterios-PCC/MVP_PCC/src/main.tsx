/**
 * main.tsx
 *
 * This is the primary entry point of your custom mapguide-react-layout viewer bundle
 */
import * as React from "react";
import * as ReactDOM from "react-dom";
import proj4 from "proj4"; 
// This is our custom application model that allows for custom reducers to be registered
import { CustomApplicationViewModel } from "./app";
// Este é o template a utilizar pelo Projecto
import PCCLayoutTemplate from "./templates/pcc-template";
// These are our custom components
import DemoComponent from "./components/demo";
import MessagesComponent from "./components/messages";
import { PreviewPlotContainer } from "./components/previewplot/previewplot";
import { ContainerBaseLayerSwitcherContainer } from "./components/baselayers/container_baselayerswitcher";
import { ContainerCoordinateTrackerContainer } from "./components/coordinatetracker/container_coordinatetracker";
import { Container_AddManageLayersContainer } from "./components/addmanagelayers/container_addmanagelayers";
import { showModalComponent, showModalUrl } from 'mapguide-react-layout/lib/actions/modal';
import { buildTargetedCommand } from 'mapguide-react-layout/lib/api/default-commands';
import { openUrlInTarget } from 'mapguide-react-layout/lib/api/registry/command';

import {
    bootstrap,
    registerLayout,
    AjaxViewerLayout,
    SidebarLayout,
    AquaTemplateLayout,
    GenericLayout,
    TurquoiseYellowTemplateLayout,
    LimeGoldTemplateLayout,
    SlateTemplateLayout,
    MaroonTemplateLayout,
    registerComponentFactory,
    registerCommand,
    SPRITE_INVOKE_SCRIPT,
    CommandConditions,
    initDefaultCommands,
    registerDefaultComponents,
    getRelativeIconPath,
    setAssetRoot,
    LayoutCapabilities,
    registerMapGuideComponents,
    initMapGuideCommands,
    DefaultComponentNames,
    MapGuideMapProviderContext,
    MapProviderContextProvider,
    MapViewer
} from "mapguide-react-layout";

import { MapAgentRequestBuilder } from 'mapguide-react-layout/lib/api/builders/mapagent';
import { addFormatDriver } from "mapguide-react-layout/lib/api/layer-manager/driver-registry";
import { FormatDriver } from "mapguide-react-layout/lib/api/layer-manager/format-driver";
import { CsvFormatDriver, CSV_COLUMN_ALIASES } from "mapguide-react-layout/lib/api/layer-manager/csv-driver";
import { registerRequestBuilder } from "mapguide-react-layout/lib/api/builders/factory";

import GeoJSON from 'ol/format/GeoJSON';
import TopoJSON from 'ol/format/TopoJSON';
import KML from 'ol/format/KML';
import GPX from 'ol/format/GPX';
import IGC from 'ol/format/IGC';

// This will pull in and embed the core stylesheet into the viewer bundle
require("mapguide-react-layout/src/styles/index.css");
// Pull in required thirdparty css
import "ol/ol.css";
import "@blueprintjs/core/lib/css/blueprint.css";
import "react-splitter-layout/lib/index.css";

// Sets up the required core libraries
bootstrap();

// Register default format drivers to use for the External Layer Manager tool
// If you have no intention of using this tool or using the Layer Manager APIs
// you can comment these lines out
addFormatDriver(new CsvFormatDriver(CSV_COLUMN_ALIASES));
addFormatDriver(new FormatDriver("GeoJSON", new GeoJSON()));
addFormatDriver(new FormatDriver("TopoJSON", new TopoJSON()));
addFormatDriver(new FormatDriver("KML", new KML(), "EPSG:4326"));
addFormatDriver(new FormatDriver("GPX", new GPX(), "EPSG:4326"));
addFormatDriver(new FormatDriver("IGC", new IGC()));

// Register the templates we intend to provide in this viewer. Below is the standard set
//
// If you don't intend to use certain templates, you can remove the registration call
// (and their respective import statement above), and the templates will not be included
// in the viewer bundle (with some size reduction as a result)
const DEFAULT_CAPS: LayoutCapabilities = {
    hasTaskPane: false
};
registerLayout("ajax-viewer", () => <AjaxViewerLayout />, DEFAULT_CAPS);
registerLayout("sidebar", () => <SidebarLayout />, DEFAULT_CAPS);
registerLayout("aqua", () => <AquaTemplateLayout />, DEFAULT_CAPS);
registerLayout("turquoise-yellow", () => <TurquoiseYellowTemplateLayout />, DEFAULT_CAPS);
registerLayout("limegold", () => <LimeGoldTemplateLayout />, DEFAULT_CAPS);
registerLayout("slate", () => <SlateTemplateLayout />, DEFAULT_CAPS);
registerLayout("maroon", () => <MaroonTemplateLayout />, DEFAULT_CAPS);
registerLayout("generic", () => <GenericLayout />, {
    hasTaskPane: false
});

// Registar o template do Projecto
registerLayout("pcc-template", () => <PCCLayoutTemplate />, DEFAULT_CAPS);

// Register the default set of commands (zoom/pan/etc/etc) to the command registry
initDefaultCommands();
initMapGuideCommands();

export const SPRITE_PRINT = "print";
export const DEFAULT_MODAL_SIZE: [number, number] = [350, 500];
registerCommand("APLICACOES", {
    iconClass: SPRITE_PRINT,
    selected: () => false,
    enabled: () => true,
    invoke: (dispatch, getState) => {
        dispatch(showModalUrl({
            modal: {
                title: "APLICACOES",
                backdrop: true,
                size: DEFAULT_MODAL_SIZE
            },
            name: "Help",
            url: "help/index.html"
        }));
    }
});
 //Preview Plot
 registerCommand("PreviewPlot", {
    iconClass: SPRITE_PRINT,
    selected: () => false,
    enabled: state => !state.stateless,
    invoke: (dispatch, getState, _viewer, parameters) => {
        const config = getState().config;
        const url = "component://PreviewPlot";
        const cmdDef = buildTargetedCommand(config, parameters);
        openUrlInTarget("PreviewPlot", cmdDef, config.capabilities.hasTaskPane, dispatch, url);
    }
});
// Register the default set of components
registerDefaultComponents();
registerMapGuideComponents();
// Register our MapGuide-specific viewer implementation
const PROVIDER_IMPL = new MapGuideMapProviderContext();
registerComponentFactory(DefaultComponentNames.Map, (props) => <MapProviderContextProvider value={PROVIDER_IMPL}>
    <MapViewer {...props} />
</MapProviderContextProvider>);
// Register our custom component. Registering a custom component allows the component to be:
//
//  1. Usable and accessible by name in a PlaceholderComponent (generally required if building custom viewer templates)
//  2. An eligible candidate component when using component:// pseudo-URIs. You can create an InvokeURL commmand with 
//     the URL of component://DemoComponent and running the command will render the DemoComponent into the TaskPane or new window
registerComponentFactory("DemoComponent", () => <DemoComponent />);
registerComponentFactory("MessagesComponent", () => <MessagesComponent />);
registerComponentFactory("BaseMapSwitcher", (props) => <ContainerBaseLayerSwitcherContainer {...props} />);
registerComponentFactory("CoordinateTracker", (props) => <ContainerCoordinateTrackerContainer {...props} />);
registerComponentFactory("AddManageLayers", (props) => <Container_AddManageLayersContainer {...props} />);
registerComponentFactory("PreviewPlot", (props) => <PreviewPlotContainer {...props} />);
 
//Register the default mapagent request builder (that can be replaced later on if desired)
registerRequestBuilder("mapagent", (agentUri, locale) => new MapAgentRequestBuilder(agentUri, locale));

// The following statements below are required if you wish to provide the same browser globals API 
// that the default viewer bundle provides. 

// Export external libraies under the MapGuide.Externals namespace
export const Externals = {
    proj4: proj4,
    React: React,
    ReactDOM: ReactDOM
};

// Export the MapGuide.Application entry point class. Note that we're exporting our custom application model
// instead of the standard ApplicationViewModel as CustomApplicationViewModel introduces custom application
// state reducers that are demonstrated by the sample application using this bundle
export { CustomApplicationViewModel as Application };

export { setAssetRoot };

