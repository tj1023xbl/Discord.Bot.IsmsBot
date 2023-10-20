import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios, { AxiosRequestConfig } from 'axios';
import Saying from "../models/Saying";
import { Status } from "./Store";

export interface IsmState {
    status: Status;
    error: string | null;
    value: Saying[];
}

/**
 * Execute API request to gat all sayings for a server
 * @param guildId 
 * @returns 
 */
const getAllSayingAsync = async (guildId: string) => {
    const url = '/api/Isms/GetAllSayings/' + guildId
    const getAllSayingsResponse = await axios({
        method: 'get',
        url: url,
        responseType: 'json',
        headers: {
            'Content-Type': 'application-json'
        }
    });
    return getAllSayingsResponse.data
}

/**
 * Async thunk that gets all sayings from the API
 */
export const getAllSayingsAsyncThunk = createAsyncThunk(
    'thunks/getAllSayingsAsyncThunk',
    async (guildId: string) => {
        return await getAllSayingAsync(guildId);
    }
)

export const deleteIsmAPICallAsyncThunk = createAsyncThunk(
    'thunks/deleteIsmAPICallAsync',
    async (saying: Saying) => {
        const url = '/api/Isms/' + saying.id;
        const response = await axios({
            method: 'delete',
            url: url,
            responseType: 'json',
            headers: {
                'Content-Type': 'application-json'
            }
        })
        const sayings = await getAllSayingAsync(saying.guildId);
        return sayings;
    });

export const addNewIsmAPICallAsyncThunk = createAsyncThunk(
    'thunks/addNewIsmAPICallAsyncThunk',
    async (saying: Saying) => {
        const url = 'api/Isms/' + 'addnewism';
        const response = await axios({
            url: url,
            method: 'post',
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            data: saying
        });
    

            //url, JSON.stringify(saying), {
            //headers: { "Content-Type": "application-json" }

        //} as AxiosRequestConfig);
        const sayings = await getAllSayingAsync(saying.guildId);
        return sayings;
    }
)

export const IsmSlice = createSlice({
    name: 'IsmListSlice',
    initialState: {
        status: 'idle',
        value: [],
        error: null
    } as IsmState,
    reducers: {

    },
    extraReducers: (builder) => {
        // GET SAYINGS
        builder.addCase(getAllSayingsAsyncThunk.pending, (state) => {
            state.status = 'loading';
            state.error = null;
            state.value = [];
        });
        builder.addCase(getAllSayingsAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null;
            state.status = 'complete';
            state.value = action.payload;
        });
        builder.addCase(getAllSayingsAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message;
            state.status = 'failed';
            state.value = [];
        });

        // DELETE SAYINGS
        builder.addCase(deleteIsmAPICallAsyncThunk.pending, (state) => {
            state.status = 'loading';
            state.error = null;
        });
        builder.addCase(deleteIsmAPICallAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null;
            state.status = 'complete';
            state.value = action.payload;
        });
        builder.addCase(deleteIsmAPICallAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message;
            state.status = 'failed';
        });

        // ADD SAYINGS
        builder.addCase(addNewIsmAPICallAsyncThunk.pending, (state) => {
            state.status = 'loading';
            state.error = null;
        });
        builder.addCase(addNewIsmAPICallAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null;
            state.status = 'complete';
            state.value = action.payload;
        });
        builder.addCase(addNewIsmAPICallAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message;
            state.status = 'failed';
        });

    }
})

