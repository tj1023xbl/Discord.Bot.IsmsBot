import { useSelector } from "react-redux"
import { RootState } from "../data/store/Store"
import { Backdrop, CircularProgress, Container } from "@mui/material"

export const Loading = () => {

    const loading = useSelector((state: RootState) => state.LoadingState)

    return (

        <Backdrop open={loading}>
            <center>
                <CircularProgress size={100} />
            </center>
        </Backdrop>

    )



}