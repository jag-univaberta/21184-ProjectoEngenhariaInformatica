import * as React from "react";
import { IExternalBaseLayer } from "mapguide-react-layout/lib/api/common";
import { STR_EMPTY, strIsNullOrEmpty } from "mapguide-react-layout/lib/utils/string";
import { tr } from "mapguide-react-layout/lib/api/i18n";

/**
 * BaseLayersSwitcher component props
 *
 * @export
 * @interface IComponentBaseLayerSwitcherProps
 */
export interface IComponentBaseLayerSwitcherProps {
    locale: string | undefined;
    externalBaseLayers: IExternalBaseLayer[];
    onBaseLayerChanged?: (name: string) => void;
}

/**
 * The BaseLayerSwitcher component provides a user interface for switching the active external
 * base layer of the current map
 * @param props 
 */
export const ComponentBaseLayerSwitcher = (props: IComponentBaseLayerSwitcherProps) => {
    const { locale, externalBaseLayers } = props;
    const visLayers = externalBaseLayers.filter(layer => layer.visible === true);
    const [selected, setSelected] = React.useState(visLayers.length == 1 ? visLayers[0].name : STR_EMPTY);
    const onBaseLayerChanged = (e: any) => {
        const value = e.currentTarget.value;
        setSelected(value);
        props.onBaseLayerChanged?.(value);
    }
    React.useEffect(() => {
        setSelected(visLayers.length == 1 ? visLayers[0].name : STR_EMPTY);
    }, [visLayers]);
    /*return <div className="status-bar-component component-base-layer-switcher-display">
        <div className="base-layer-switcher-item-container">
            <label className="bp3-control bp3-radio">
                <input className="base-layer-switcher-option" type="radio" value={STR_EMPTY} checked={strIsNullOrEmpty(selected)} onChange={onBaseLayerChanged} />
                <span className="bp3-control-indicator" />
                {tr("NONE", locale)}
            </label>
        </div>
        {externalBaseLayers.map(layer => {
            return <div className="base-layer-switcher-item-container" key={`base-layer-${layer.name}`}>
                <label className="bp3-control bp3-radio">
                    <input className="base-layer-switcher-option" type="radio" value={layer.name} checked={layer.name === selected} onChange={onBaseLayerChanged} />
                    <span className="bp3-control-indicator" />
                    {layer.name}
                </label>
            </div>;
        })}
    </div>;*/

return (
    <div className="status-bar-component component-base-layer-switcher-display">
      <label htmlFor="base-layer-select">Mapa Base : </label>
      <select
        id="base-layer-select"
        className="base-layer-switcher-option"
        value={selected}
        onChange={onBaseLayerChanged}
      >
        <option value={STR_EMPTY}> Nenhum </option>
        {externalBaseLayers.map((layer) => (
          <option key={`base-layer-${layer.name}`} value={layer.name}>
            {layer.name}
          </option>
        ))}
      </select>
    </div>
  );
}