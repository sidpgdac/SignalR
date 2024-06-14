import * as signalR from "@microsoft/signalr";

const URL = process.env.REACT_APP_HUB_ADDRESS ?? "https://localhost:5001/hub"; // or whatever your backend port is

class Connector {
    private connection: signalR.HubConnection;
    public events: (onDataPointReceived: (stockName: string, value: number) => void) => void;
    static instance: Connector;

    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(URL)
            .withAutomaticReconnect()
            .build();
        this.connection.start().catch(err => console.error(err));
        this.events = (onDataPointReceived) => {
            this.connection.on("dataPointReceived", (stockName: string, value: number) => {
                onDataPointReceived(stockName, value);
            });
        };
    }

    public static getInstance(): Connector {
        if (!Connector.instance) {
            Connector.instance = new Connector();
        }
        return Connector.instance;
    }
}

export default Connector.getInstance();
