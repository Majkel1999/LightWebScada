package main

import (
	"context"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"

	"github.com/jackc/pgx/v4/pgxpool"
)

var db *pgxpool.Pool

type DiscreteRegister struct {
	RegisterNumber int
	CurrentValue   bool
}

type ValueRegister struct {
	RegisterNumber int
	CurrentValue   int
}

type DataSet struct {
	CoilRegisters    []DiscreteRegister
	DiscreteInputs   []DiscreteRegister
	InputRegisters   []ValueRegister
	HoldingRegisters []ValueRegister
}

func ingestionHandler(w http.ResponseWriter, r *http.Request) {
	if r.Method == "POST" {

		key := r.URL.Query().Get("ApiKey")
		if key != "test" {
			w.WriteHeader(401)
			w.Write([]byte("401 - wrong authentication"))
			return
		}

		body, err := ioutil.ReadAll(r.Body)
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			w.Write([]byte("500 - internal server error"))
			return
		}

		var data DataSet
		json.Unmarshal([]byte(body), &data)
		fmt.Fprintf(w, "POST succesfull\n")
		for _, element := range data.CoilRegisters {
			fmt.Fprintf(w, "Register %d, Value %t \n", element.RegisterNumber, element.CurrentValue)
		}
		for _, element := range data.DiscreteInputs {
			fmt.Fprintf(w, "Register %d, Value %t \n", element.RegisterNumber, element.CurrentValue)
		}
		for _, element := range data.InputRegisters {
			fmt.Fprintf(w, "Register %d, Value %d \n", element.RegisterNumber, element.CurrentValue)
		}
		for _, element := range data.HoldingRegisters {
			fmt.Fprintf(w, "Register %d, Value %d \n", element.RegisterNumber, element.CurrentValue)
		}
		_, err = db.Exec(context.Background(), `INSERT INTO public."TestTable" (data) VALUES ('Siema')`)
		if err != nil {
			panic(err)
		}
	} else {
		fmt.Fprintf(w, "Not a POST")
	}
}

func main() {
	psqlInfo := "postgres://ingest_server:ingest@serwer.lan:49155/ScadaData"
	var err error
	db, err = pgxpool.Connect(context.Background(), psqlInfo)
	if err != nil {
		panic(err)
	}
	defer db.Close()
	err = db.Ping(context.Background())
	if err != nil {
		panic(err)
	}
	fmt.Println("Connected to database")
	http.HandleFunc("/send/", ingestionHandler)
	log.Fatal(http.ListenAndServe(":8080", nil))
}
