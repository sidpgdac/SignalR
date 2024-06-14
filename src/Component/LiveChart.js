import React, { useEffect, useState } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from '@mui/charts';
import Connector from './Connector';

const LiveChart = () => {
    const [chartData, setChartData] = useState([]);

    useEffect(() => {
        const { events } = Connector;

        events((stockName, value) => {
            setChartData(prevData => {
                const newData = [...prevData];
                const index = newData.findIndex(data => data.name === stockName);
                if (index !== -1) {
                    newData[index].data.push(value);
                }
                return newData;
            });
        });

        return () => {
            // Clean up event listener
        };
    }, []);

    return (
        <div>
            <h2>Live Stock Market Chart</h2>
            <LineChart width={800} height={400} data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Legend />
                {Object.keys(chartData).map((stockName, index) => (
                    <Line
                        key={index}
                        type="monotone"
                        dataKey={`data[${index}]`}
                        stroke={`#${(Math.random() * 0xFFFFFF << 0).toString(16)}`}
                        name={stockName}
                    />
                ))}
            </LineChart>
        </div>
    );
};

export default LiveChart;
