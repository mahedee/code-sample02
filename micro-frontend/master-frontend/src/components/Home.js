import React from "react";
import "../assets/homeStyle.css";

export const Home = () => {
  return (
    <>
      <div style={{ marginTop: "20px" }}>
        <div className="container">
          <div className="span1"></div>
          <div className="span2">
            <h2 className="medtext">Think</h2>

            <h2 className="large">Simple</h2>
          </div>
          <div className="span3">
            <h2>MAHEDEE.NET</h2>
          </div>
          <div className="span4">
            <h2>Learn</h2>
          </div>

          <div className="span5"></div>
          <div className="span6"></div>
          <div className="span7"></div>
        </div>

        <div className="overflow">
          <h1 className="hero">MAHEDEE.NET</h1>
        </div>
      </div>
    </>
  );
};
