import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 1000,
    duration: '60s',
};

export default function () {
    
    http.get('http://localhost:5283/api/v1/post');
    sleep(1);
}