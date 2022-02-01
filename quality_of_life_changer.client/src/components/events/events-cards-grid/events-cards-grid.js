import { Component } from "react";
import { Row } from "react-bootstrap";
import EventCard from "../event-card/event-card";
import "./events-cards-grid.css";

class EventsCardsGrid extends Component {
  render() {
    console.log("props in grid - ", this.props.events);
    console.log(this.props.events);
    return (
      <div className="custom-grid">
        <Row md="auto" sm="auto">
          {this.props.events.map((x) => (
            <EventCard
              key={x.id}
              name={x.name}
              owner={x.owner}
              startTime={x.startDateTime}
              endTime={x.endDateTime}
            />
          ))}
        </Row>
      </div>
    );
  }
}

export default EventsCardsGrid;
