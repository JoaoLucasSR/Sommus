import "./NumberBox.css";

function NumberBox(props) {
    return (
        <div className="number-box">
            <h3>{props.title}</h3>
            <p>Media Confirmados: {props.confirmed}</p>
            <p>Media Mortos: {props.deaths}</p>
        </div>
    );
}

export default NumberBox;