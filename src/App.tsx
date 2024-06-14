import React, { useEffect, useState } from 'react';
import './App.css';
import Connector from './signalr-connection';

type StockName = "AAPL" | "GOOGL" | "MSFT" | "AMZN" | "TSLA";

type StockData = {
  [key in StockName]: number | null;
};

const stockNames: StockName[] = ["AAPL", "GOOGL", "MSFT", "AMZN", "TSLA"];
const stockColors: { [key in StockName]: string } = {
  AAPL: "#ff6b6b",
  GOOGL: "#4285F4",
  MSFT: "#34a853",
  AMZN: "#ff9900",
  TSLA: "#cc0000"
};

function App() {
  const { events } = Connector;
  const [stockData, setStockData] = useState<StockData>({
    AAPL: null,
    GOOGL: null,
    MSFT: null,
    AMZN: null,
    TSLA: null,
  });

  useEffect(() => {
    events((stockName: StockName, value: number) => {
      setStockData(prevData => ({
        ...prevData,
        [stockName]: value
      }));
    });
  }, [events]);

  return (
    <div className="App">
      <h1>Real-Time Stock Market Data</h1>
      <div className="card-container">
        {stockNames.map((stock) => (
          <div className="card" key={stock} style={{ borderColor: stockColors[stock] }}>
            <h2>{stock}</h2>
            <p>{stockData[stock] !== null ? stockData[stock]?.toFixed(2) : "Loading..."}</p>
          </div>
        ))}
      </div>
    </div>
  );
}

export default App;
