import { createSlice } from "@reduxjs/toolkit";
import { getAllSayingsAsyncThunk } from './IsmSlice'
import { getAllGuildsAsyncThunk } from "./GuildSlice";

export const LoadingSlice = createSlice({
    name: 'LoadingSlice',
    initialState: false,
    reducers: {
        setLoadingState: (state, action) => {
            return action.payload
        }
    },
    extraReducers: (builder) => {
        builder.addCase(getAllSayingsAsyncThunk.pending, (state) => {
            return true
        })
        builder.addCase(getAllGuildsAsyncThunk.pending, (state) => {
            return true
        })
        builder.addDefaultCase(state => {
            return false
        })

    }
})

export const { setLoadingState } = LoadingSlice.actions;