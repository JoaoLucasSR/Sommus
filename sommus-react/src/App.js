import React from "react";
import './App.css';
import "./components/NumberBox";
import NumberBox from './components/NumberBox';

class App extends React.Component {

  state = {
    model1: null,
    model2: null,
    model3: null
  };

  componentWillMount() {
    let current_datetime = new Date();
    // eslint-disable-next-line
    let formatted_date = current_datetime.getFullYear() + "-" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + "T" + "00:00:00Z";
    fetch('http://localhost:5000/api/MediaMovel?date='+formatted_date)
      .then(response => response.json())
      .then(data => this.setState({model1: data}));

    current_datetime = new Date(new Date().setDate(new Date().getDate() - 7));
    // eslint-disable-next-line
    formatted_date = current_datetime.getFullYear() + "-" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + "T" + "00:00:00Z";
    fetch('http://localhost:5000/api/MediaMovel?date='+formatted_date)
      .then(response => response.json())
      .then(data => this.setState({model2: data}));

    current_datetime = new Date(new Date().setDate(new Date().getDate() - 14));
    // eslint-disable-next-line
    formatted_date = current_datetime.getFullYear() + "-" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + "T" + "00:00:00Z";
    fetch('http://localhost:5000/api/MediaMovel?date='+formatted_date)
      .then(response => response.json())
      .then(data => this.setState({model3: data}));
  }

  render() {
    if(this.state.model1 === null || this.state.model2 === null || this.state.model3 === null)
    {
      return(
        <div className="App">
          <h1>Loading ...</h1>   
        </div>
      );
    }

    return(
      <div className="App">
        <NumberBox title="Esta Semana" confirmed={this.state.model1.confirmed} deaths={this.state.model1.deaths} />
        <NumberBox title="Semana Passada" confirmed={this.state.model2.confirmed} deaths={this.state.model2.deaths} />
        <NumberBox title="Semana Retrasada" confirmed={this.state.model3.confirmed} deaths={this.state.model3.deaths} />
      </div>
    );
  }
}

export default App;
