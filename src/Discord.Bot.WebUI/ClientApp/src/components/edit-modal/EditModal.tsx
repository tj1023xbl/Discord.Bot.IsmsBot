import { Modal } from "@mui/base";
import { DateRange } from "@mui/icons-material";
import { Autocomplete, Dialog, Popover, TextField } from "@mui/material";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { useEffect, useState } from "react";
import Saying from "../../data/models/Saying";
import styles from './EditModal.module.scss';
import moment from "moment";

const sx = {
    margin: '10px 0 10px 0'
}

const EditModal = ({ isModalOpen, saying, handleClose, ismKeyList, recorderList }: { isModalOpen: boolean, saying: Saying, handleClose: () => void, ismKeyList: Set<string>, recorderList: Set<string> }) => {
    const [open, setOpen] = useState(false);
    const [_internalSaying, setInternalSaying] = useState<Saying>(saying);

    const handleChange = (change: {}) => {
        setInternalSaying(state => {
            return {
                ...state,
                ...change
            }
        })
    }

    useEffect(() => {
        setInternalSaying(saying);
        setOpen(isModalOpen);
    }, [saying])

    return (
        <>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description">

                <section className={styles.container}>
                    <Autocomplete
                        options={Array.from(ismKeyList)}
                        value={_internalSaying.ismKey}
                        freeSolo
                        onChange={(e, value) => handleChange({ ismKey: value })}
                        renderInput={(params) => <TextField { ...params } sx={sx} label="Whose ism is it?"   />} />
                    <TextField sx={sx} multiline label="Ism Saying" value={_internalSaying.ismSaying} />
                    <Autocomplete
                        options={Array.from(recorderList)}
                        freeSolo
                        value={_internalSaying.ismRecorder}
                        onChange={(e, value) => handleChange({ ismRecorder: value })}
                        renderInput={(params) => <TextField {...params} sx={sx} label="Who recorded the ism?"  />} />
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker sx={sx} label="When was the ism recorded?" value={moment(_internalSaying.dateCreated)} />
                    </LocalizationProvider>
                </section>

            </Dialog>

        </>
    )

}

export default EditModal;