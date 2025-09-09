import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { Icon } from '@fluentui/react';


class SearchComponent extends Component {
  constructor(props) {
    super(props);
    
    this.state = {
      searchTerm: '',
      autocomplete: [],
      isFocused: false,
    };
  }

  handleInputChange = (event) => {
    this.setState({ searchTerm: event.target.value });

    this.fetchData(event.target.value);
  };
  handleKeyDown = (event) => {
    if (event.keyCode === 27) {
      // Escape key was pressed
      // Clear the input value or perform any desired action
      this.setState({ autocomplete: [], searchTerm: '' });
    }
  };
  async fetchData(pesquisa) {
    if (pesquisa!=''){
      const apiEndpoint = this.props.apiEndpoint;
      const url = apiEndpoint + 'Autocomplete/' + pesquisa;
      const response = await fetch(url);
      const json = await response.json();
      this.setState({ autocomplete: json });
    } else {
      this.setState({ autocomplete: [] });
    }

  }

  handleSearch = (event) => {
    event.preventDefault();
    // Perform search operation using the search term
    console.log('Searching for:', this.state.searchTerm);

    this.fetchData();
  };
 
  handleClickItem = (locationX, locationY) => {
    // Execute your action using locationX and locationY
    console.log('Clicked item with locationX:', locationX, 'and locationY:', locationY);
    var viewerstate = viewer.getState();
    //Get active map name
    var activeMapName = viewerstate.config.activeMapName;
    const escala = 5000;
    if (locationX!=null && locationY!= null && escala !=null){

      var aux={ type:'Map/SET_VIEW', payload: { mapName: activeMapName,  view: { x: locationX, y: locationY, scale: escala } }};
      viewer.dispatch(aux); 
    }

  };
  handleFocus = () => {
    this.setState({ isFocused: true });
  };

  handleBlur = () => {


    if (this.state.searchTerm ==''){
      this.setState({ isFocused: false });
    } else {
      this.setState({ isFocused: true });
    }
    
  };
  handleClickBack= () => {
    this.setState({ isFocused: false , autocomplete: [], searchTerm: '' });
    
  };
  handleClickZoom= () => {
    this.inputRef.focus();
  };
  handleClickPesquisa= () => {
    this.inputRef.focus();
  };
  render() {
    return (
      <div className="app_pesquisa1">
        <div className="app_pesquisachild"> 
          <form onSubmit={this.handleSearch}>
            <div className="search-container">
              
              <div className="wrap-search-ele" onClick={this.state.isFocused ? this.handleClickBack : this.handleClickZoom} 
              title={this.state.isFocused ? 'Sair da pesquisa' : ''}
              >  
                <i className="wrap-search-ele1">
                  <span className="wrap-search-ele2">
                  <i className="wrap-search-ele3">
                    <Icon iconName={this.state.isFocused ? 'Back' : 'Zoom'}
                     style={{ fontSize: '16px', width: '16px', height:'16px' }} />
                    
                  </i>
                  </span>
                </i>
              </div>

              <div className="search-wrapeleinput1"> 
                <div className="search-wrapeleinput">
                  <input className="search-eleinput"
                    type="text"
                    value={this.state.searchTerm}
                    onChange={this.handleInputChange}
                    onKeyDown={this.handleKeyDown}
                    onFocus={this.handleFocus} 
                    onBlur={this.handleBlur} 
                    placeholder="Procurar"
                    ref={(input) => { this.inputRef = input; }}
                  /> 

                </div>
              </div>
              <div  className={`wrap-search-ele ${this.state.isFocused ? 'wrap-search-elevisible' : 'wrap-search-elehidden'}`}
                    onClick={this.handleClickPesquisa}>
                <i className="wrap-search-ele1">
                  <span className="wrap-search-ele2">
                  <i className="wrap-search-ele3">
                    <Icon iconName='Zoom' style={{ fontSize: '16px', width: '16px', height:'16px' }} />
                    
                  </i>
                  </span>
                </i>
              </div>
            </div>
          </form>
        </div> 
        <div className="search-result">
          {this.state.autocomplete && this.state.autocomplete.map((item) => (
                <div
                key={item.id}
                className="search-item"
                onClick={() => this.handleClickItem(item.geom.x, item.geom.y)}
                >
                {item.texto}
              </div>
          ))}
        </div>
      </div> 
      
    );
  }
}

export default SearchComponent;
