import * as React from "react";
import { ComponentBaseLayerSwitcher } from "./component_baselayerswitcher";
import { useActiveMapName, useViewerLocale, useActiveMapExternalBaseLayers } from 'mapguide-react-layout/lib/containers/hooks';
import { setBaseLayer } from 'mapguide-react-layout/lib//actions/map';
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";

export interface IContainerBaseLayerSwitcherContainerProps {

}

export const ContainerBaseLayerSwitcherContainer = () => {
    const mapName = useActiveMapName();
    const locale = useViewerLocale();
    const externalBaseLayers = useActiveMapExternalBaseLayers(false);
    const dispatch = useReduxDispatch();
    const setBaseLayerAction = (mapName: string, layerName: string) => dispatch(setBaseLayer(mapName, layerName));
    const onBaseLayerChanged = (layerName: string) => {
        if (mapName) {
            setBaseLayerAction?.(mapName, layerName);
        }
    }
    if (locale && externalBaseLayers) {
        return <ComponentBaseLayerSwitcher onBaseLayerChanged={onBaseLayerChanged} externalBaseLayers={externalBaseLayers} locale={locale} />;
    } else {
        return <noscript />;
    }
}