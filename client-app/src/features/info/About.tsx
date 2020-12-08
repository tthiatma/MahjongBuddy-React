import React, { Fragment } from "react";
import { Segment } from "semantic-ui-react";

const About = () => {
  return (
    <Fragment>
      <Segment>
        <h1>About</h1>

        <p>
          Contact me through email at <a href="mailto:info@mahjongbuddy.com">info@mahjongbuddy.com</a>.
        </p>
      </Segment>
    </Fragment>
  );
};

export default About;
